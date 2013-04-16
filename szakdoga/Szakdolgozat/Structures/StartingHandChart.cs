using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structures
{
    public class StartingHandChart
    {

        private String[,] table;
        private String[] ranks;
        private List<String> p62;

        public List<String> P62
        {
            get { return p62; }
            set { p62 = value; }
        }
        private List<String> p58;

        public List<String> P58
        {
            get { return p58; }
            set { p58 = value; }
        }
        private List<String> p55;

        public List<String> P55
        {
            get { return p55; }
            set { p55 = value; }
        }
        private List<String> p52;

        public List<String> P52
        {
            get { return p52; }
            set { p52 = value; }
        }
        private List<String> p50;

        public List<String> P50
        {
            get { return p50; }
            set { p50 = value; }
        }
            
        
        
        public StartingHandChart()
        {
            
		    String[,] table2 = new String[14,14];
		    String[] ranks2 = { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };	

		    List<String> _p62 = new List<String>();
            List<String> _p58 = new List<String>();
            List<String> _p55 = new List<String>();
            List<String> _p52 = new List<String>();
            List<String> _p50 = new List<String>();
		    
            
            for(int i = 0; i < 14; i++ )
		    {
			    for(int j = 0; j < 14; j++ )
			    {
                    if (i == 0 && j > 0) table2[0, j] = ranks2[j - 1];
				    if( j == 0 && i > 0)  table2[i,0] = ranks2[i-1];
                }
            }

            for(int i = 1; i < 14; i++ )
		    {
			    for(int j = 1; j < 14; j++ )
			    {
				    table2[i,j] = table2[0,j]  + table2[i,0];

                }
            }

            String[] data = { "AA", "AK", "AQ", "AJ", "A10", "A9", "A8", "AKO", "AQO", "AJO", "A10O", "KQ", "KJ", "KK", "QQ", "JJ", "1010", "99", "88", "77", "66"};
            _p62.AddRange(data);
            String[] data2 = { "55", "A7", "A6", "A5", "A4", "A3", "K10", "K9", "K8", "QJ", "Q10", "A9O", "A8O", "A7O", "KQO", "KJO", "K10O", "QJO"};
            _p58.AddRange(_p62);
            _p58.AddRange(data2);
            String[] data3 = { "A6O", "A5O", "A4O", "A3O", "K9O", "K8O", "K7O", "Q10O", "44", "A2", "K7", "K6", "K5", "Q9", "Q8", "J9", "J10"};
            _p55.AddRange(_p58);
            _p55.AddRange(data3);
            String[] data4 = { "33", "K4", "K3", "K2", "Q7", "Q6", "Q5", "J8", "109", "A2O", "K6O", "K5O", "K4O", "Q9O", "Q8O", "J10O", "J9" };
            _p52.AddRange(_p55);
            _p52.AddRange(data4);
            String[] data5 = { "Q4", "Q3", "Q2", "22", "J7", "J6", "J5", "108", "107", "98", "K3O", "K2O", "Q7O", "Q6O", "Q5O", "J8O", "109O" };
            _p50.AddRange(_p52);
            _p50.AddRange(data5);


            this.p62 = _p62;
            this.p58 = _p58;
            this.p55 = _p55;
            this.p52 = _p52;
            this.p50 = _p50;
            this.table = table2;
            this.ranks = ranks2;

        }



        public String[,] getTable()
        {
            return this.table;
        }



    }
}
