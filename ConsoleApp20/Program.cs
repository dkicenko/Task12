using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;



namespace task_12_non_chain_productions
{
    class Program
    {

        const string fname = "test.txt";
        static int count_of_grammar = helper.grammar_lines(fname);
        static string[] nonterminals = new string[count_of_grammar]; // all nonterminals
        static string[] chain_productions = new string[count_of_grammar]; // all chain productions
        static string[] old_grammar = new string[count_of_grammar]; // old grammar (load from file)
        static string[] new_grammar = new string[count_of_grammar]; //new result grammar

        public class helper// all helper functions
        {
            public static int grammar_lines(string p) // counts all lines in old grammar fron file
            {
                int K = 0;
                using (StreamReader s = new StreamReader(p))
                {
                    string line;
                    while ((line = s.ReadLine()) != null)
                    {
                        K++;
                    }
                }
                return K;
            }

            public static void filereader()
            {
                try
                {
                    using (StreamReader sread = new StreamReader(fname))
                    {
                        int i = 0;
                        string ss;
                        while ((ss = sread.ReadLine()) != null)
                        {
                            Console.WriteLine(ss);
                            old_grammar[i] = ss;
                            nonterminals[i] = "" + ss[0];
                            chain_productions[i] = ss[0] + " ";
                            i++;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }


            public static void distinct_adder(int i)
            {

                string[] splitted = chain_productions[i].Split(' ');
                chain_productions[i] = null;
                var ditinct_splitted = splitted.Distinct();// make distinct splitted productions for adding
                foreach (var x in ditinct_splitted)
                    chain_productions[i] += x + " ";// add distinct 


            }


        }




        public class chainfounder// all functions connected to algorythm
        {
            public static void chain_finder(int i)// finds chains
            {
                try
                {
                    string s = old_grammar[i];// one string from old grammer
                    string[] split_line = s.Split(' ');// we split it 
                    foreach (string str in split_line)
                    {
                        if (nonterminals.Contains(str))
                        {
                            chain_productions[i] += str + " ";//add to list of chain productions
                            for (int j = 0; j < old_grammar.Length; j++) //go deeper
                            {
                                string ss = old_grammar[j];//continue to split
                                if (("" + ss[0]).CompareTo(str) == 0)
                                {
                                    chain_finder(j);// recursive call
                                    chain_productions[i] += chain_productions[j];// add new chain producrion to the list of chain productions
                                }

                            }
                        }
                        else
                        {
                            new_grammar[i] += str + " ";// else if we have not found chains we add symbols to the new grammar
                        }
                    }

                    helper.distinct_adder(i);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);// if the file does not exist
                }

            }


            public static void chain_deleter_and_builder()
            {
                try
                {
                    for (int i = 0; i < chain_productions.Length; i++)
                    {
                        var splitter = chain_productions[i].Split(' ');// take 1 chain production from the list
                        for (int j = 1; j < splitter.Length; j++)// foreach production
                        {
                            string str = splitter[j]; // take one
                            foreach (var x in new_grammar)// and here we build bew grammar by changing all chain productions
                            {
                                if ((x[0] + "").CompareTo(str) == 0)
                                {
                                    var helping_grammar = x.Split(' ');
                                    for (int k = 1; k < helping_grammar.Length; k++)
                                    {
                                        new_grammar[i] += helping_grammar[k] + " ";
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);// if the file does not exist
                }


            }

        }


        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(" Old grammar: ");
                helper.filereader();
                chainfounder.chain_finder(0);
                chainfounder.chain_deleter_and_builder();

                Console.WriteLine("New grammar");
                foreach (var x in new_grammar)
                {
                    Console.WriteLine(x);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);// if the file does not exist
            }


        }
    }
}
