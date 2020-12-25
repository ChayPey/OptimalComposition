using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptimalComposition.Models
{
    public class ComponentInputValue
    {
        /// <summary>
        /// Имя компонента
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Процент Si
        /// </summary>
        public double PercentSi { get; set; }
        /// <summary>
        /// Процент Mn
        /// </summary>
        public double PercentMn { get; set; }
        /// <summary>
        /// Стоимость, усл.ед./т
        /// </summary>
        public double Cost { get; set; }
        /// <summary>
        /// Минимальное колличество компонента
        /// </summary>
        public double MinPercent { get; set; }
        /// <summary>
        /// Максимальное колличество компонента
        /// </summary>
        public double MaxPercent { get; set; }
    }
}
