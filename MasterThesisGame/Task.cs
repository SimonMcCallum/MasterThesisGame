using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace MasterThesisGame
{
    public class Task
    {
        public struct MandatoryLexem
        {
            public string Text;
            public byte Count;

            public MandatoryLexem(string text, byte count)
            {
                Text = text;
                Count = count;
            }
        }

        public int Number { get; set; }
        public byte Score { get; set; }
        public bool IsFinished { get; set; }
        public string Text { get; set; }
        public string Info { get; set; }
        public List<Point> Points { get; protected set; }
        public List<MandatoryLexem> MandatoryLexems { get; protected set; }
        public List<string> VisibleInformation { get; protected set; }

        public Task()
        {
            Score = 5;
            Points = new List<Point>();
            MandatoryLexems = new List<MandatoryLexem>();
            VisibleInformation = new List<string>();
        }

        public void Save(string path)
        {
            using (Stream stream = File.OpenWrite(path))
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    bw.Write(Number);
                    bw.Write(Text);
                    bw.Write(Info);
                    bw.Write(Points.Count);

                    for (int i = 0; i < Points.Count; i++)
                    {
                        bw.Write(Points[i].X);
                        bw.Write(Points[i].Y);
                    }

                    bw.Write(MandatoryLexems.Count);

                    for (int i = 0; i < MandatoryLexems.Count; i++)
                    {
                        bw.Write(MandatoryLexems[i].Text);
                        bw.Write(MandatoryLexems[i].Count);
                    }

                    bw.Write(VisibleInformation.Count);

                    for (int i = 0; i < VisibleInformation.Count; i++)
                        bw.Write(VisibleInformation[i]);
                }
            }
        }

        public static Task Load(string path)
        {
            Task task = new Task();

            using (Stream stream = File.OpenRead(path))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    task.Number = br.ReadInt32();
                    task.Text = br.ReadString();
                    task.Info = br.ReadString();

                    int count = br.ReadInt32();

                    for (int i = 0; i < count; i++)
                        task.Points.Add(new Point(br.ReadInt32(), br.ReadInt32()));

                    count = br.ReadInt32();

                    for (int i = 0; i < count; i++)
                        task.MandatoryLexems.Add(new MandatoryLexem(br.ReadString(), br.ReadByte()));

                    count = br.ReadInt32();

                    for (int i = 0; i < count; i++)
                        task.VisibleInformation.Add(br.ReadString());
                }
            }

            return task;
        }
    }
}
