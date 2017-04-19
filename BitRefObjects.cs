using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRef {
    public class SymbolLocal {
        public string code { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public double conversion { get; set; }
        public bool symbolAppearsAfter { get; set; }
        public bool local { get; set; }
    }

    public class SymbolBtc {
        public string code { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public double conversion { get; set; }
        public bool symbolAppearsAfter { get; set; }
        public bool local { get; set; }
    }

    public class LatestBlock {
        public int block_index { get; set; }
        public string hash { get; set; }
        public int height { get; set; }
        public int time { get; set; }
    }

    public class Info {
        public int nconnected { get; set; }
        public double conversion { get; set; }
        public SymbolLocal symbol_local { get; set; }
        public SymbolBtc symbol_btc { get; set; }
        public LatestBlock latest_block { get; set; }
    }

    public class Wallet {
        public int n_tx { get; set; }
        public int n_tx_filtered { get; set; }
        public long total_received { get; set; }
        public long total_sent { get; set; }
        public long final_balance { get; set; }
    }

    public class Address {
        public string address { get; set; }
        public int n_tx { get; set; }
        public long total_received { get; set; }
        public long total_sent { get; set; }
        public long final_balance { get; set; }
        public int change_index { get; set; }
        public int account_index { get; set; }
    }

    public class PrevOut {
        public object value { get; set; }
        public int tx_index { get; set; }
        public int n { get; set; }
        public bool spent { get; set; }
        public string script { get; set; }
        public int type { get; set; }
        public string addr { get; set; }
    }

    public class Input {
        public object sequence { get; set; }
        public string script { get; set; }
        public PrevOut prev_out { get; set; }
    }

    public class Out {
        public object value { get; set; }
        public int tx_index { get; set; }
        public int n { get; set; }
        public bool spent { get; set; }
        public string script { get; set; }
        public int type { get; set; }
        public string addr { get; set; }
    }

    public class Tx {
        public string hash { get; set; }
        public int ver { get; set; }
        public int vin_sz { get; set; }
        public int vout_sz { get; set; }
        public int size { get; set; }
        public string relayed_by { get; set; }
        public int lock_time { get; set; }
        public int tx_index { get; set; }
        public bool double_spend { get; set; }
        public long result { get; set; }
        public object balance { get; set; }
        public int time { get; set; }
        public int block_height { get; set; }
        public List<Input> inputs { get; set; }
        public List<Out> @out { get; set; }
    }

    public class BitRefResponse {
        public bool recommend_include_fee { get; set; }
        public string sharedcoin_endpoint { get; set; }
        public Info info { get; set; }
        public Wallet wallet { get; set; }
        public List<Address> addresses { get; set; }
        public List<Tx> txs { get; set; }
    }
}
