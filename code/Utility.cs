using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideShow.TCP_IP.WinForm.Concrete
{
    class Utility
    {
        public object GetRandom<T>(IList<T> pool)
        {
            if (pool == null)
            {
                throw new Exception("GetRandom:  no pool to pull from.");
            }
            if (pool.Count == 0)
            {
                throw new Exception("GetRandom:  pool is empty");
            }
            int? randomId = null;
            Random randomGenerator = new Random();
            randomId = randomGenerator.Next(0, pool.Count - 1);
            if (randomId == null)
            {
                throw new Exception("GetRandom: random id is still null");
            }
            return (T)pool[(int)randomId];
        }
    }
}
