using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptimalComposition.Models
{
    /// <summary>
    /// Результаты расчета массы компонентов
    /// </summary>
    public class ComponentResult
    {
        /// <summary>
        /// Имя компонента
        /// </summary>
        public string Name;
        /// <summary>
        /// Масса коспонента
        /// </summary>
        public double FullMass;
        /// <summary>
        /// Масса Si
        /// </summary>
        public double SiMass;
        /// <summary>
        /// Масса Mn
        /// </summary>
        public double MnMass;
        /// <summary>
        /// Масса компонета в процентах от общей
        /// </summary>
        public double PercentMass;
        /// <summary>
        /// Стоимость, усл.ед./т
        /// </summary>
        public double Cost;
    }
}
