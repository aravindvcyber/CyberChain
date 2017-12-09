using System;
using System.Collections.Generic;
using System.IO;

namespace CyberChain
{
    public class CyberChain
    {

        public List<Block> Chain = new List<Block>();
        public int Difficulty { get; set; }

        public static List<string> fileEntries=new List<string>();

        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public string ChainFailureAt()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currBlock = Chain.Find(b => b.Index == i);
                Block prevBlock = Chain.Find(b => b.Index == i - 1);
                if (currBlock.Hash != currBlock.ComputeHash())
                {
                    return currBlock.Index.ToString() + " due to corrupt hash.";

                }
                if (currBlock.PreviousHash != prevBlock.Hash)
                {
                    return currBlock.Index.ToString() + " due to mismatch with previous block hash.";

                }

            }
            return " Does not find any blocks corrupted.";
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            fileEntries.Add(path);
        }

        public CyberChain()
        {
            this.Difficulty = 4;
            createGenesisBlock();
            string rootFolder = Path.Combine(Environment.CurrentDirectory, "data");
            if (!Directory.Exists(rootFolder)) Directory.CreateDirectory(rootFolder);

            ProcessDirectory(Path.Combine(Environment.CurrentDirectory, "data"));

            if (fileEntries.Count > 0)
            {
                // Read the file contents back into a variable.


                foreach (var file in fileEntries)
                {
                    Block blk = Serialization.ReadFromJsonFile<Block>(file);
                  Chain.Add(blk);
                }
                fileEntries.Clear();

            }
            else
            {
            }



        }
        public void  createGenesisBlock()
        {
            addBlock(new Block(0, Convert.ToDateTime("01/01/2001"), "Cyber Chain Genesis"));
        }
        public Block getLatestBlock()
        {
            
            return Chain.Find(b=>b.Index==Chain.Count-1);
        }
        public void addBlock(Block NewBlock)
        {
            if (Chain.Count == 0)
                NewBlock.PreviousHash = "000000000000";
            else
                NewBlock.PreviousHash = this.getLatestBlock().Hash;
           
            NewBlock.mineBlock(this.Difficulty);
            Chain.Add(NewBlock);
            if (!(Chain.Count <2))
                storeBlockInChain(NewBlock);
        }
        

        private void storeBlockInChain(Block blk)
        {
            // Write the contents of the variable someClass to a file.
            string rootFolder = Path.Combine(Environment.CurrentDirectory, "data");

            string folder = Path.Combine(rootFolder, blk.Hash.Substring(0, 2), blk.Hash.Substring(2, 2), blk.Hash.Substring(4, 2), blk.Hash.Substring(6, 2), blk.Hash.Substring(8, 2), blk.Hash.Substring(10, 2), blk.Hash.Substring(12, 2), blk.Hash.Substring(14, 2), blk.Hash.Substring(16, 2), blk.Hash.Substring(18, 2), blk.Hash.Substring(20, 2), blk.Hash.Substring(22, 2));
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, blk.Hash + ".json");
            Serialization.WriteToJsonFile<Block>(file, blk);


        }

        public Boolean isChainValid()
        {
           
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currBlock = Chain.Find(b=>b.Index==i); 
                Block prevBlock = Chain.Find(b => b.Index == i-1);
                if (currBlock.Hash != currBlock.ComputeHash())
                {
                    return false;

                }
                if (currBlock.PreviousHash != prevBlock.Hash)
                {
                    return false;
                }

            }
            return true;
        }



    }
}
