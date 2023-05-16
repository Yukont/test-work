using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tess
{
    public class Matrix3D
    {
        private double[,] matrix;

        public Matrix3D()
        {
            matrix = new double[4, 4];
            matrix[0, 0] = 1.0;
            matrix[1, 1] = 1.0;
            matrix[2, 2] = 1.0;
            matrix[3, 3] = 1.0;
        }

        public double this[int row, int col]
        {
            get { return matrix[row, col]; }
            set { matrix[row, col] = value; }
        }
        public static Matrix3D operator *(Matrix3D m1, Matrix3D m2)
        {
            Matrix3D result = new Matrix3D();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < 4; k++)
                    {
                        result[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }
            return result;
        }
        public void TransformPoint(Point3D p)
        {
            double x = p.X * matrix[0, 0] + p.Y * matrix[1, 0] + p.Z * matrix[2, 0] + matrix[3, 0];
            double y = p.X * matrix[0, 1] + p.Y * matrix[1, 1] + p.Z * matrix[2, 1] + matrix[3, 1];
            double z = p.X * matrix[0, 2] + p.Y * matrix[1, 2] + p.Z * matrix[2, 2] + matrix[3, 2];
            double w = p.X * matrix[0, 3] + p.Y * matrix[1, 3] + p.Z * matrix[2, 3] + matrix[3, 3];

            if (w != 0)
            {
                p.X = x / w;
                p.Y = y / w;
                p.Z = z / w;
            }
        }
        public static Matrix3D CreateRotationY(double radians)
        {
            Matrix3D m = new Matrix3D();

            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            m.matrix[0,0] = m.matrix[2,2] = cos;
            m.matrix[0,2] = sin;
            m.matrix[2,0] = -sin;

            return m;
        }

        public static Matrix3D CreateRotationX(double radians)
        {
            Matrix3D m = new Matrix3D();

            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            m.matrix[1,1] = m.matrix[2,2] = cos;
            m.matrix[1,2] = -sin;
            m.matrix[2,1] = sin;

            return m;
        }

        public static Matrix3D CreateRotationZ(double radians)
        {
            Matrix3D m = new Matrix3D();

            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            m.matrix[0,0] = m.matrix[1,1] = cos;
            m.matrix[0,1] = -sin;
            m.matrix[1,0] = sin;

            return m;
        }

        public static Matrix3D CreateFromYawPitchRoll(double yaw, double pitch, double roll)
        {
            return (/*CreateRotationY(yaw) */CreateRotationX(pitch)) * CreateRotationZ(roll);
        }

        public static Matrix3D CreateTranslation(Point3D position)
        {
            Matrix3D m = new Matrix3D();

            m.matrix[0,3] = position.X;
            m.matrix[1,3] = position.Y;
            m.matrix[2,3] = position.Z;

            return m;
        }
    }
}
