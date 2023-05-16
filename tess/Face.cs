using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tess
{
    public class Face
    {
        public List<Point3D> Vertices { get; set; }
        public Color Color { get; set; }

        public Face(List<Point3D> vertices, Color color)
        {
            Vertices = vertices;
            Color = color;
        }
    }
}
