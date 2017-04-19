using Bit2C.Tester.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BitRef {
    public static class infra {
        public static IEnumerable<List<T>> splitList<T>(this List<T> locations, int nSize = 10) {
            for (int i = 0; i < locations.Count; i += nSize) {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }

    public static class BitRef {

        public static List<addressBalance> GetAllBalanceByDate(List<string> addresses) {
            var splitArray = addresses.splitList(1);
            List<addressBalance> total = new List<addressBalance>();
            int index = 0;
            foreach (var item in splitArray) {
                index++;
                List<string> chunk = item.Cast<string>().ToList();
                List<addressBalance> lst = GetBalanceByDate(chunk);
                total.AddRange(lst);
                System.Threading.Thread.Sleep(2000);
            }
            return total;
        }

        public static List<addressBalance> GetBalanceByDate(List<string> addresses) {
            //https://blockchain.info/multiaddr?cors=true&active=1F1xcRt8H8Wa623KqmkEontwAAVqDSAWCV|1Ngv5Zm43xJLSMY9dsHzHu4NkffZFmfJwt
            string url = string.Format("https://blockchain.info/multiaddr?cors=true&active={0}", string.Join("|", addresses));
            WebClient wc = new WebClient();
            string html = wc.DownloadString(url);
            BitRefResponse brResponse = Deserialize<BitRefResponse>(html);
            List<addressBalance> addressesBalance = new List<addressBalance>();

            

            if (brResponse == null) {
                Console.Write("! address?");
            }

            
            foreach (Tx tx in brResponse.txs) {
                string addressFetched = string.Empty;
                decimal balance = 0;
                DateTime inDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(tx.time);
                foreach (Input input in tx.inputs) {
                    if (input.prev_out != null && addresses.Contains(input.prev_out.addr)) {
                        addressFetched = input.prev_out.addr;
                        balance = (Convert.ToInt64( tx.balance)) / 100000000m;
                    }
                }
                foreach (Out outTx in tx.@out) {
                    if (addresses.Contains(outTx.addr)) {
                        addressFetched = outTx.addr;
                        balance = (Convert.ToInt64(tx.balance)) / 100000000m;
                    }
                }

                if (!string.IsNullOrEmpty(addressFetched)) {
                    addressBalance ab = new addressBalance();
                    ab.address = addressFetched;
                    ab.balance = balance;
                    ab.inDate = inDate;
                    addressesBalance.Add(ab);
                }
            }
            return addressesBalance;
        }


        public static decimal GetBalance(List<string> addresses,bool writeToLog = false) {
            var splitArray = addresses.splitList(10);
            decimal total = 0;
            foreach (var item in splitArray) {
                string[] array = item.Cast<string>().ToArray();
                total += getTotalBalance(array, writeToLog);
                System.Threading.Thread.Sleep(1000);
            }
            return total;
        }
        private static decimal getTotalBalance(string[] addresses, bool writeToLog = false) {
            //https://blockchain.info/multiaddr?cors=true&active=1F1xcRt8H8Wa623KqmkEontwAAVqDSAWCV|1Ngv5Zm43xJLSMY9dsHzHu4NkffZFmfJwt
            string url = string.Format("https://blockchain.info/multiaddr?cors=true&active={0}", string.Join("|", addresses));
            WebClient wc = new WebClient();
            string html = wc.DownloadString(url);
            BitRefResponse brResponse = Deserialize<BitRefResponse>(html);

            if (writeToLog) {
                string path = @"C:\Users\Bitusi\Documents\bit2c\btc-balance.txt";
                using (StreamWriter sw = File.AppendText(path)) {
                    foreach (var item in brResponse.addresses) {
                        sw.WriteLine(string.Format("{0} {1}", item.address, item.final_balance / 100000000m));
                    }
                }	
            }

            if (brResponse == null) {
                Console.Write("! address?");
            }


            long sum = (from c in brResponse.addresses
                        select c.final_balance).Sum();
            decimal balance = sum / 100000000m;
            return balance;
        }

        public static T Deserialize<T>(string json) {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

    }
}
