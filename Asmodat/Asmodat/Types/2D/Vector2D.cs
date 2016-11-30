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

namespace Asmodat.Types
{
    /// <summary>
    /// This strunct is smatr version on vector, consistion of two points, Start the beggining of vector and End, end of vector
    /// </summary>
    [Serializable]
    [DataContract(Name = "vector_2d")]
    public class Vector2D
    {
        public Vector2D()
        {
            this.Start = Point2D.Default;
            this.End = this.Start;
        }

        public Vector2D(Point2D Start, Point2D End)
        {
            this.Start = Start;
            this.End = End;
        }

        [XmlElement("start")]
        public Point2D Start;
        [XmlElement("end")]
        public Point2D End;

        /// <summary>
        /// This property checks if vector value is invalid
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                if (Start.IsInvalid || End.IsInvalid)
                    return true;
                else return false;
            }
        }

        public static Vector2D Default
        {
            get
            {
                return new Vector2D(Point2D.Default, Point2D.Default);
            }
        }

        /// <summary>
        /// Rests vector to default value
        /// </summary>
        public void Reset()
        {
            Start.Reset();
            End.Reset();
        }


        public double MinX
        {
            get
            {
               return Math.Min(Start.X, End.X);
            }
        }
        public double MaxX
        {
            get
            {
                return Math.Max(Start.X, End.X);
            }
        }
        public double MinY
        {
            get
            {
                return Math.Min(Start.Y, End.Y);
            }
        }
        public double MaxY
        {
            get
            {
                return Math.Max(Start.Y, End.Y);
            }
        }
        public double Min
        {
            get
            {
                return Math.Min(MinX, MinX);
            }
        }
        public double Max
        {
            get
            {
                return Math.Max(MaxX, MaxX);
            }
        }

        /// <summary>
        /// |Start.X - End.X|
        /// </summary>
        public double Width
        {
            get
            {
                return Math.Abs(Start.X - End.X);
            }
        }

        /// <summary>
        /// |Start.Y - End.Y|
        /// </summary>
        public double Height
        {
            get
            {
                return Math.Abs(Start.Y - End.Y);
            }
        }
    }
}
