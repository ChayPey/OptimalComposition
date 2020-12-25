using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptimalComposition.Models
{
    public class Results
    {
        /// <summary>
        /// Результаты расчета всех компонентов
        /// </summary>
        public List<ComponentResult> ComponentResults = new List<ComponentResult>();
        /// <summary>
        /// Общие колличество составляющих компонентов
        /// </summary>
        public ComponentResult AllComponentResult = new ComponentResult();

        /// <summary>
        /// Вычисляет общие колличество составляющих компонентов
        /// </summary>
        /// <returns>Общие колличество составляющих компонентов</returns>
        public ComponentResult AllResult()
        {
            ComponentResult allComponentResult = new ComponentResult() { Name = "Всего" };
            foreach (var item in ComponentResults)
            {
                allComponentResult.FullMass += item.FullMass;
                allComponentResult.SiMass += item.SiMass;
                allComponentResult.MnMass += item.MnMass;
                allComponentResult.PercentMass += item.PercentMass;
                allComponentResult.Cost += item.Cost;
            }
            return allComponentResult;
        }
    }
}
