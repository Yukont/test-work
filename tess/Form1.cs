using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tess
{
    public partial class Form1 : Form
    {
        private const int cubeSize = 100; // Размер куба
        private const int bigCubeSize = cubeSize * 2; // Размер большого куба
        private const int distance = 300; // Расстояние от фигуры до наблюдателя

        List<Face> faces = new List<Face>(); // Лист граней куба
        List<Face> bigFaces = new List<Face>(); // Лист граней большого куба

        private Point3D[] cubePoints; // Массив точек куба
        private Point3D[] bigCubePoints; // Массив точек большого куба
        private Point lastMousePos; // Последняя позиция мыши

        Matrix3D perspectiveMatrix = new Matrix3D();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Инициализируем массив точек куба
            cubePoints = new Point3D[8];
            cubePoints[0] = new Point3D(-cubeSize, -cubeSize, -cubeSize);
            cubePoints[1] = new Point3D(-cubeSize, -cubeSize, cubeSize);
            cubePoints[2] = new Point3D(cubeSize, -cubeSize, cubeSize);
            cubePoints[3] = new Point3D(cubeSize, -cubeSize, -cubeSize);
            cubePoints[4] = new Point3D(-cubeSize, cubeSize, -cubeSize);
            cubePoints[5] = new Point3D(-cubeSize, cubeSize, cubeSize);
            cubePoints[6] = new Point3D(cubeSize, cubeSize, cubeSize);
            cubePoints[7] = new Point3D(cubeSize, cubeSize, -cubeSize);

            // Инициализируем массив точек большого куба
            bigCubePoints = new Point3D[8];
            bigCubePoints[0] = new Point3D(-bigCubeSize, -bigCubeSize, -bigCubeSize);
            bigCubePoints[1] = new Point3D(-bigCubeSize, -bigCubeSize, bigCubeSize);
            bigCubePoints[2] = new Point3D(bigCubeSize, -bigCubeSize, bigCubeSize);
            bigCubePoints[3] = new Point3D(bigCubeSize, -bigCubeSize, -bigCubeSize);
            bigCubePoints[4] = new Point3D(-bigCubeSize, bigCubeSize, -bigCubeSize);
            bigCubePoints[5] = new Point3D(-bigCubeSize, bigCubeSize, bigCubeSize);
            bigCubePoints[6] = new Point3D(bigCubeSize, bigCubeSize, bigCubeSize);
            bigCubePoints[7] = new Point3D(bigCubeSize, bigCubeSize, -bigCubeSize);

            // Назначаем обработчик события Paint для перерисовки формы
            //this.Paint += new PaintEventHandler(Form1_Paint);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            // Очищаем форму
            g.Clear(Color.White);

            // Сдвигаем начало координат в центр формы
            g.TranslateTransform(this.ClientSize.Width / 2, this.ClientSize.Height / 2);

            // Применяем перспективное преобразование координат
            perspectiveMatrix[3, 2] = -1.0 / distance;
            foreach (Point3D p in cubePoints)
            {
                perspectiveMatrix.TransformPoint(p);
            }
            foreach (Point3D p in bigCubePoints)
            {
                perspectiveMatrix.TransformPoint(p);
            }
            
            // Создаем грани
            var bottomFace = new Face(
                new List<Point3D> { cubePoints[0], cubePoints[1], cubePoints[2], cubePoints[3] }, Color.Red);
            var topFace = new Face(
                new List<Point3D> { cubePoints[4], cubePoints[5], cubePoints[6], cubePoints[7] }, Color.Blue);
            var leftFace = new Face(
                new List<Point3D> { cubePoints[0], cubePoints[4], cubePoints[7], cubePoints[3] }, Color.Green);
            var rightFace = new Face(
                new List<Point3D> { cubePoints[1], cubePoints[5], cubePoints[6], cubePoints[2] }, Color.Orange);
            var backFace = new Face(
                new List<Point3D> { cubePoints[0], cubePoints[1], cubePoints[5], cubePoints[4] }, Color.Yellow);
            var frontFace = new Face(
                new List<Point3D> { cubePoints[3], cubePoints[2], cubePoints[6], cubePoints[7] }, Color.Purple);

            faces.Clear();

            // Добавляем грани
            faces.Add(bottomFace);
            faces.Add(topFace);
            faces.Add(leftFace);
            faces.Add(rightFace);
            faces.Add(backFace);
            faces.Add(frontFace);

            // Создаем грани большого куба
            var bigBottomFace = new Face(
                new List<Point3D> { bigCubePoints[0], bigCubePoints[1], bigCubePoints[2], bigCubePoints[3] }, Color.Red);
            var bigTopFace = new Face(
                new List<Point3D> { bigCubePoints[4], bigCubePoints[5], bigCubePoints[6], bigCubePoints[7] }, Color.Blue);
            var bigLeftFace = new Face(
                new List<Point3D> { bigCubePoints[0], bigCubePoints[4], bigCubePoints[7], bigCubePoints[3] }, Color.Green);
            var bigRightFace = new Face(
                new List<Point3D> { bigCubePoints[1], bigCubePoints[5], bigCubePoints[6], bigCubePoints[2] }, Color.Orange);
            var bigBackFace = new Face(
                new List<Point3D> { bigCubePoints[0], bigCubePoints[1], bigCubePoints[5], bigCubePoints[4] }, Color.Yellow);
            var bigFrontFace = new Face(
                new List<Point3D> { bigCubePoints[3], bigCubePoints[2], bigCubePoints[6], bigCubePoints[7] }, Color.Purple);

            bigFaces.Clear();

            // Добавляем грани большого куба
            bigFaces.Add(bigBottomFace);
            bigFaces.Add(bigTopFace);
            bigFaces.Add(bigLeftFace);
            bigFaces.Add(bigRightFace);
            bigFaces.Add(bigBackFace);
            bigFaces.Add(bigFrontFace);


            // Сортируем грани по удаленности от наблюдателя (по оси Z)
            faces = faces.OrderBy(f => f.Vertices.Average(p => p.Z)).ToList();
            bigFaces = bigFaces.OrderBy(f => f.Vertices.Average(p => p.Z)).ToList();

            // Отрисовываем грани в правильном порядке
            foreach (var face in faces)
            {
                DrawFace(g, face.Vertices[0], face.Vertices[1], face.Vertices[2], face.Vertices[3], Color.BlueViolet, 125);
            }
            foreach (var face in bigFaces)
            {
                DrawFace(g, face.Vertices[0], face.Vertices[1], face.Vertices[2], face.Vertices[3], Color.Black, 65);
            }
            for (int i = 0; i < 8; i++)
            {
                DrawConnectionLines(g, cubePoints[i], bigCubePoints[i]);
            }
        }
        private void DrawFace(Graphics g, Point3D p1, Point3D p2, Point3D p3, Point3D p4, Color color, int transparency)
        {
            Point[] points = new Point[4];
            points[0] = new Point((int)p1.X, (int)p1.Y);
            points[1] = new Point((int)p2.X, (int)p2.Y);
            points[2] = new Point((int)p3.X, (int)p3.Y);
            points[3] = new Point((int)p4.X, (int)p4.Y);

            // Закрашиваем грань цветом
            //Brush brush = new SolidBrush(Color.FromArgb(transparency, color));
            //g.FillPolygon(brush, points);

            // Рисуем контур грани
            //Pen pen = new Pen(Color.DeepPink);
            Pen pen = new Pen(color);
            g.DrawPolygon(pen, points);
        }
        private void DrawConnectionLines(Graphics g, Point3D p1, Point3D p2)
        {
            Point[] points = new Point[2];
            points[0] = new Point((int)p1.X, (int)p1.Y);
            points[1] = new Point((int)p2.X, (int)p2.Y);

            // Рисуем контур грани
            Pen pen = new Pen(Color.DarkBlue);
            //Pen pen = new Pen(color);
            g.DrawLine(pen, points[0], points[1]);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Запоминаем текущую позицию мыши
            lastMousePos = e.Location;
        }
        
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            double rotationSpeed = 0.0;
            if (e.Button == MouseButtons.Left)
            {
                // Вычисляем разницу между текущей позицией мыши и предыдущей
                int dx = e.X - lastMousePos.X;
                int dy = e.Y - lastMousePos.Y;

                // Обновляем матрицу преобразования
                rotationSpeed = Math.Sqrt(dx * dx + dy * dy) * 0.001;
                double radiansX = dy * Math.PI / 180.0 * rotationSpeed;
                double radiansY = dx * Math.PI / 180.0 * rotationSpeed;
                var rotateM = Matrix3D.CreateFromYawPitchRoll(0, radiansY, radiansX);

                perspectiveMatrix = rotateM * perspectiveMatrix;
                // Перерисовываем форму
                Invalidate();

                // Запоминаем текущую позицию мыши
                lastMousePos = e.Location;
            }
        }
    }
    
}
