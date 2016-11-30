using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

using Asmodat.Abbreviate;

using System.Threading;
using System.Runtime.Serialization;

using System.Drawing;

namespace Asmodat.Types
{

    [Serializable]
    [DataContract(Name = "point_2d")]
    public partial  struct Point2D
    {

        public Point2D(double X, double Y)  : this()
        {
            this.X = X;
            this.Y = Y;
        }




        [XmlElement("x")]
        public double X;
        [XmlElement("y")]
        public double Y;

        /// <summary>
        /// This property checks if point value is invalid
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                if (Double.IsNaN(X) || Double.IsNaN(Y))
                    return true;
                else return false;
            }
        }

        /// <summary>
        /// This property contains default value of point
        /// </summary>
        public static Point2D Default
        {
            get
            {
                return new Point2D(double.NaN, double.NaN);
            }
        }

        /// <summary>
        /// Resets point value to default
        /// </summary>
        public void Reset()
        {
            Point2D def = Default;
            this.X = def.X;
            this.Y = def.Y;
        }


        public static explicit operator Point(Point2D point)
        {
            return new Point((int)point.X, (int)point.Y);
        }
        public static explicit operator Point2D(Point point)
        {
            return new Point2D(point.X, point.Y);
        }

    }
}
