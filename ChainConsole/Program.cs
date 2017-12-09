using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CyberChain;
using Newtonsoft.Json;

namespace ChainConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Console Started @ " + DateTime.Now.ToString());

            CyberChain.CyberChain Chain = new CyberChain.CyberChain();
            Console.WriteLine("Validating Chain @ " + DateTime.Now.ToString());
            Console.WriteLine("Validation Status : " + Chain.isChainValid());
            if(!(Chain.isChainValid()))
                Console.WriteLine("Found Chain Failure on Block with Index of " + Chain.ChainFailureAt());
            int start = Chain.Chain.Count;
            for (int i =start ; i < start+6; i++)
            {
                Block blk = new Block(Chain.Chain.Count, DateTime.Now, (i * i * i * i * i).ToString());

                Chain.addBlock(blk);
                Console.WriteLine("Mined Block "+Chain.Chain.Find(b=>b.Index== i).Index+"  : "+ Chain.Chain.Find(b => b.Index ==  i).Data);
            }
            foreach (Block blk in  Chain.Chain)
            {
                Console.WriteLine(JsonConvert.SerializeObject(
                                                            blk,
                                                            Formatting.None,
                                                            new JsonSerializerSettings()
                                                            {
                                                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                                            }
                                                            ));

            }

            Console.WriteLine("Validating Chain @ " + DateTime.Now.ToString());
            Console.WriteLine("Validation Status : " + Chain.isChainValid());

            Console.WriteLine("Console Waiting @ " + DateTime.Now.ToString());
            Console.ReadLine();

        }
    }
}
