using System;

using System.Text;

using System.Security.Cryptography;

namespace CyberChain
{
    [Serializable]
    public  class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public int Nonce { get; set; }
        public string Data { get; set; }
        public string Hash { get; set; }

        public Block()
        { }
        public Block(int index, DateTime timeStamp, string data)

        {
            this.Index = index;
            this.TimeStamp = timeStamp;
            this.PreviousHash = "0000"; 
            this.Nonce = 0;
            this.Data = data;
            this.Hash = ComputeHash();
        }
        public override string ToString()
        {
            return this.Index.ToString()+" "+this.TimeStamp.ToString() + " " + this.PreviousHash.ToString() + " " + this.Data.ToString() + " " + this.Nonce.ToString();
        }
        public string ComputeHash()
        {


            using (SHA512 sha512 = new SHA512CryptoServiceProvider())
            {
                var data = Encoding.UTF8.GetBytes(this.ToString());
                return ByteArrayToString(sha512.ComputeHash(data));
            }
        }
        public void mineBlock(int difficulty)
        {
            string startZeros = "";
            for (int i = 1; i <= difficulty; i++)
            {
                startZeros += "0";
            }
            while (this.Hash.Substring(0, difficulty) != startZeros)
            {
                this.Nonce++;
                this.Hash = ComputeHash();
            }
        }
        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
