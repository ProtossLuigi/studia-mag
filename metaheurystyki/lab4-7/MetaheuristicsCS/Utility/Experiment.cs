using Optimizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class Experiment<Element>
    {
        private AOptimizer<Element> optimizer;
        private Dictionary<string, string> info;
        private Action finishListener;

        public Experiment(AOptimizer<Element> optimizer, Dictionary<string, string> info = null, Action finishListener = null)
        {
            this.optimizer = optimizer;
            if (info == null)
            {
                this.info = new Dictionary<string, string>();
            }
            else
            {
                this.info = info;
            }
            if(finishListener == null)
            {
                this.finishListener = () => { };
            }
            else
            {
                this.finishListener = finishListener;
            }
        }

        public Dictionary<string, string> Execute()
        {
            optimizer.Run();
            var result = optimizer.Result;
            info["fitness"] = result.BestValue.ToString();
            info["time"] = result.BestTime.ToString();
            info["iteration"] = result.BestIteration.ToString();
            info["FFE"] = result.BestFFE.ToString();
            finishListener();
            return info;
        }
    }
}
