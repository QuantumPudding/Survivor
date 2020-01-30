using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TileMapEditor
{
    public partial class Form1 : Form
    {
        List<Texture> Textures;
        Texture SelectedTexture;
        Rectangle selectionRect;
        Tile[,] grid;
        Graphics g;

        string path;
        string texturespath;

        int mapwidth = 25;
        int mapheight = 18;
        int tilewidth = 32;
        int tileheight = 32;
        int xoffset = 0;
        int yoffset = 0;

        int selwidth = 1;
        int selheight = 1;


        int[,] collisionLayer; 

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            mapwidth = int.Parse(txtMapWidth.Text);
            mapheight = int.Parse(txtMapHeight.Text);
            tilewidth = int.Parse(txtTileWidth.Text);
            tileheight = int.Parse(txtTileHeight.Text);

            grid = new Tile[mapwidth, mapheight];

            for (int y = 0; y < mapheight; y++)
            {
                for (int x = 0; x < mapwidth; x++)
                {
                    grid[x, y] = new Tile(
                        new Rectangle(
                            x * tilewidth,
                            y * tileheight,
                            tilewidth, tileheight), -1);
                }
            }


            //for (int y = 0; y < mapheight; y++)
            //{
            //    int pixelY = (y * tileheight) - yoffset;
            //    if (pixelY > 0)
            //        g.DrawLine(Pens.Black, new Point(0, pixelY), new Point(canvas.Width, pixelY));  
            //}

            //for (int y = 0; y < mapheight; y++)
            //{
            //    int pixelX = (x * tilewidth) - xoffset;
            //    if (pixelX > 0)
            //        g.DrawLine(Pens.Black, new Point(pixelX, 0), new Point(pixelX, canvas.Height));
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = canvas.CreateGraphics();

            txtMapWidth.Text = mapwidth.ToString();
            txtMapHeight.Text = mapheight.ToString();
            txtTileWidth.Text = tilewidth.ToString();
            txtTileHeight.Text = tileheight.ToString();

            Textures = new List<Texture>();

            grid = new Tile[mapwidth, mapheight];
            collisionLayer = new int[mapwidth, mapheight];

            for (int y = 0; y < mapheight; y++)
            {
                for (int x = 0; x < mapwidth; x++)
                {
                    grid[x, y] = new Tile(
                        new Rectangle(
                            x * tilewidth,
                            y * tileheight,
                            tilewidth, tileheight), -1);
                }
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            if (grid == null)
                return;

            for (int y = 0; y < mapheight; y++)
            {
                for (int x = 0; x < mapwidth; x++)
                {
                    Tile t = grid[x, y];

                    if (t != null)
                    {
                        if (t.Index != -1)
                            e.Graphics.DrawImage(Textures[t.Index].Image, new Rectangle(t.Bounds.X + 1 - xoffset, t.Bounds.Y + 1 - yoffset, t.Bounds.Width - 2, t.Bounds.Height - 2));

                        if (cbLayer.SelectedIndex == 1)
                        {
                            e.Graphics.FillRectangle(
                                new SolidBrush(Color.FromArgb(128, 0, 0, 0)),
                                new Rectangle(t.Bounds.X + 1 - xoffset, t.Bounds.Y + 1 - yoffset, t.Bounds.Width - 2, t.Bounds.Height - 2));

                            e.Graphics.DrawString(
                                t.CollisionIndex.ToString(),
                                new Font("Tahoma", 8, FontStyle.Regular),
                                Brushes.White,
                                new PointF(
                                    t.Bounds.X,
                                    t.Bounds.Y));
                        }
                    }
                }
            }

            e.Graphics.DrawRectangle(Pens.Blue, selectionRect);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            List<List<int>> tempLayout = new List<List<int>>();
            bool readingPath = false;
            bool readingLayout = false;
            bool readingTextures = false;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Text = dlg.FileName;

                using (StreamReader sr = new StreamReader(dlg.FileName))
                {
                    while (sr.Peek() != -1)
                    {
                        string line = sr.ReadLine().Trim();

                        if (string.IsNullOrEmpty(line))
                            continue;

                        if (line.Contains("[TexturePath]"))
                        {
                            readingPath = true;
                            readingLayout = false;
                            readingTextures = false;
                        }
                        else if (line.Contains("[Textures]"))
                        {
                            readingTextures = true;
                            readingPath = false;
                            readingLayout = false;
                        }
                        else if (line.Contains("[Layout]"))
                        {
                            readingLayout = true;
                            readingPath = false;
                            readingTextures = false;
                        }
                        else if (readingTextures)
                        {
                            string filepath = Path.Combine(path, line.Substring(line.IndexOf("/") + 1)) + ".png";
                            Textures.Add(new Texture(Image.FromFile(filepath), line));
                        }
                        else if (readingPath)
                        {
                            path = line;
                        }
                        else if (readingLayout)
                        {
                            List<int> row = new List<int>();
                            string[] cells = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string c in cells)
                                row.Add(int.Parse(c));

                            tempLayout.Add(row);
                        }
                    }
                }

                if (cbLayer.SelectedIndex == 0)
                {
                    mapwidth = tempLayout[0].Count;
                    mapheight = tempLayout.Count;
                    grid = new Tile[mapwidth, mapheight];

                    for (int y = 0; y < mapheight; y++)
                        for (int x = 0; x < mapwidth; x++)
                            grid[x, y] = new Tile(
                                new Rectangle(
                                    x * tilewidth,
                                    y * tileheight,
                                    tilewidth, tileheight),
                                    tempLayout[y][x]);

                    lbTextures.Items.Clear();
                    foreach (Texture t in Textures)
                        lbTextures.Items.Add(t.Name);

                    lbTextures.SelectedIndex = 0;

                    txtMapWidth.Text = mapwidth.ToString();
                    txtMapHeight.Text = mapheight.ToString();
                    txtTileWidth.Text = tilewidth.ToString();
                    txtTileHeight.Text = tileheight.ToString();
                }
                else
                {
                    for (int y = 0; y < mapheight; y++)
                        for (int x = 0; x < mapwidth; x++)
                            grid[x, y].CollisionIndex = tempLayout[y][x];
                }

                canvas.Invalidate();
            } 
        }

        private void lbTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Texture t in Textures)
                if (t.Name == lbTextures.SelectedItem.ToString())
                    SelectedTexture = t;
        }

        bool mousedown = false;
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (grid != null)
            {
                foreach (Tile t in grid)
                {
                    if (t.Bounds.Contains(canvas.PointToClient(Cursor.Position)))
                    {
                        selectionRect = new Rectangle(
                            t.Bounds.X,
                            t.Bounds.Y,
                            t.Bounds.Width * selwidth,
                            t.Bounds.Height * selheight);
                    }
                }

                foreach (Tile t in grid)
                {
                    if (selectionRect.Contains(t.Bounds))
                    {
                        if (cbLayer.SelectedIndex == 0)
                        {
                            if (mousedown && lbTextures.SelectedItem != null)
                            {
                                if (e.Button == MouseButtons.Left)
                                    t.Index = lbTextures.Items.IndexOf(lbTextures.SelectedItem);
                                else
                                    t.Index = -1;
                            }
                        }
                        else
                        {
                            
                        }
                    }
                }

                canvas.Invalidate();
            }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            mousedown = true;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mousedown = false;
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (grid != null)
            {
                foreach (Tile t in grid)
                {
                    if (t.Bounds.Contains(canvas.PointToClient(Cursor.Position)))
                    {
                        selectionRect = new Rectangle(
                            t.Bounds.X,
                            t.Bounds.Y,
                            t.Bounds.Width * selwidth,
                            t.Bounds.Height * selheight);
                    }
                }

                foreach (Tile t in grid)
                {
                    if (selectionRect.Contains(t.Bounds))
                    {
                        if (cbLayer.SelectedIndex == 0)
                        {
                            if (lbTextures.SelectedItem != null)
                            {
                                if (e.Button == MouseButtons.Left)
                                    t.Index = lbTextures.Items.IndexOf(lbTextures.SelectedItem);
                                else
                                    t.Index = -1;
                            }
                        }
                        else
                        {
                            if (e.Button == MouseButtons.Left)
                                t.CollisionIndex++;
                            else
                                t.CollisionIndex--;
                        }
                    }
                }

                canvas.Invalidate();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                yoffset -= tileheight;
            if (e.KeyCode == Keys.Left)
                xoffset -= tilewidth;
            if (e.KeyCode == Keys.Down)
                yoffset += tileheight;
            if (e.KeyCode == Keys.Right)
                xoffset += tilewidth;

            canvas.Invalidate();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = null;
            if (path == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    path = dlg.FileName;
                    FileInfo f = new FileInfo(path);
                    name = f.Name;
                }
            }

            if (path == null)
                return;

            if (cbLayer.SelectedIndex == 0)
            {
                if (name == null)
                    name = this.Text;

                StreamWriter sw = new StreamWriter(name, false);

                sw.WriteLine("[TexturePath]");
                sw.WriteLine(path);
                sw.WriteLine();

                sw.WriteLine("[Textures]");
                foreach (Texture t in Textures)
                    sw.WriteLine(t.Name);

                sw.WriteLine();
                sw.WriteLine("[Layout]");
                for (int y = 0; y < mapheight; y++)
                {
                    string line = "";
                    for (int x = 0; x < mapwidth; x++)
                        line += grid[x, y].Index.ToString() + " ";

                    sw.WriteLine(line);
                }

                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            else
            {
                StreamWriter sw = new StreamWriter(Path.Combine(this.Text.Remove(this.Text.LastIndexOf("\\")), "Collisions"), false);

                sw.WriteLine("[Layout]");
                for (int y = 0; y < mapheight; y++)
                {
                    string line = "";
                    for (int x = 0; x < mapwidth; x++)
                        line += grid[x, y].CollisionIndex.ToString() + " ";

                    sw.WriteLine(line);
                }

                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        private void txtSelWidth_TextChanged(object sender, EventArgs e)
        {
            if (txtSelWidth.Text != "")
                selwidth = int.Parse(txtSelWidth.Text);
        }

        private void txtSelHeight_TextChanged(object sender, EventArgs e)
        {
            if (txtSelHeight.Text != "")
                selheight = int.Parse(txtSelHeight.Text);
        }
    }

    public class Tile
    {
        public Rectangle Bounds;
        public int Index;
        public int CollisionIndex;

        public Tile(Rectangle r, int i)
        {
            Bounds = r;
            Index = i;
        }
    }

    public class Texture
    {
        public Image Image;
        public string Name;

        public Texture(Image image, string name)
        {
            Image = image;
            Name = name;
        }
    }
}
