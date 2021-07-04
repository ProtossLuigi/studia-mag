using Optimizers.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class Experiment<Element, EvaluationResult, OptimizationResult>
    {
        private AOptimizer<Element, EvaluationResult, OptimizationResult> optimizer;
        private Dictionary<string, string> info;
        private Action finishListener;

        public Experiment(AOptimizer<Element, EvaluationResult, OptimizationResult> optimizer, Dictionary<string, string> info = null, Action finishListener = null)
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
            if (finishListener == null)
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
            Type type = typeof(OptimizationResult);
            if(type == typeof(Optimizers.SingleObjective.OptimizationResult<Element>))
            {
                var result = optimizer.Result as Optimizers.SingleObjective.OptimizationResult<Element>;
                info["fitness"] = result.BestValue.ToString();
                info["time"] = result.BestTime.ToString();
                info["iteration"] = result.BestIteration.ToString();
                info["FFE"] = result.BestFFE.ToString();
            }
            else if (type == typeof(Optimizers.BiObjective.OptimizationResult<Element>))
            {
                var result = optimizer.Result as Optimizers.BiObjective.OptimizationResult<Element>;
                info["HV"] = result.Front.HyperVolume().ToString();
                info["IGD"] = result.Front.InversedGenerationalDistance().ToString();
                info["time"] = result.LastUpdateTime.ToString();
                info["iteration"] = result.LastUpdateIteration.ToString();
                info["FFE"] = result.LastUpdateFFE.ToString();
            }
            else
            {
                throw new NotSupportedException();
            }
            finishListener();
            return info;
        }
    }
}
