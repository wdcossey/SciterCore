using System;
using System.Collections.Generic;
using System.Linq;
using SciterCore;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Gtk.Behaviors
{
	public class DrawGeometryBehavior : SciterEventHandler
	{

        public DrawGeometryBehavior()
        {

        }

        public DrawGeometryBehavior(string name)
            : base(name: name)
        {

        }

		protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
		{
			if (prms.cmd == SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT)
			{
				using (SciterGraphics g = new SciterGraphics(prms.gfx))
				{
					g.StateSave();
					g.Translate(prms.area.Left, prms.area.Top);

					List<Tuple<float, float>> points = new List<Tuple<float, float>>
					{
						/*Tuple.Create(51.0f, 58.0f),
						Tuple.Create(70.0f, 28.0f),
						Tuple.Create(48.0f, 1.0f),
						Tuple.Create(15.0f, 14.0f),
						Tuple.Create(17.0f, 49.0f),
                        */
						/*
                         ViewPort: 0 0 96 96

polygon(68.89 95.60, 72.82 95.16, 92.60 85.65, 96.00 80.24, 96.00 15.76, 92.60 10.35, 72.82 0.84, 66.00 2.00, 34.12 37.26, 15.50 22.00, 13.87 20.60, 10.26 19.77, 9.73 19.95, 2.46 22.97, 0.01 26.37, 0.00 26.67, 0.00 69.33, 0.01 69.63, 2.46 73.02, 9.73 76.05, 10.26 76.23, 13.87 75.40, 15.50 74.00, 34.12 58.74, 66.00 94.00, 68.89 95.60)
polygon(15.50 74.00, 13.87 75.40, 9.73 76.06, 2.46 73.03, 0.00 69.33, 0.00 26.67, 2.46 22.97, 9.73 19.95, 13.87 20.60, 15.50 22.00, 12.00 23.80, 12.00 72.20, 15.50 74.00)
polygon(96.00 15.77, 96.00 16.00, 89.81 13.07, 15.50 74.00, 13.87 75.40, 9.73 76.06, 2.46 73.03, 0.00 69.33, 0.00 69.00, 4.02 70.55, 66.00 2.00, 72.82 0.84, 92.60 10.36, 96.00 15.77)
polygon(96.00 80.00, 96.00 80.23, 92.60 85.64, 72.82 95.16, 66.00 94.00, 4.02 25.45, 0.00 27.00, 0.00 26.67, 2.46 22.97, 9.73 19.95, 13.87 20.60, 15.50 22.00, 89.81 82.93, 96.00 80.00)
polygon(66.00 94.00, 66.44 94.38, 66.00 94.00)
                         */
					};

					var x = new double[] { 68.89, 95.60, 72.82, 95.16, 92.60, 85.65, 96.00, 80.24, 96.00, 15.76, 92.60, 10.35, 72.82, 0.84, 66.00, 2.00, 34.12, 37.26, 15.50, 22.00, 13.87, 20.60, 10.26, 19.77, 9.73, 19.95, 2.46, 22.97, 0.01, 26.37, 0.00, 26.67, 0.00, 69.33, 0.01, 69.63, 2.46, 73.02, 9.73, 76.05, 10.26, 76.23, 13.87, 75.40, 15.50, 74.00, 34.12, 58.74, 66.00, 94.00, 68.89, 95.60 };

                    for (int i = 0; i < x.Length; i++)
                    {
						points.Add(Tuple.Create(Convert.ToSingle(x[i]), Convert.ToSingle(x[++i])));

					}

                    //g.LineColor = new RGBAColor(0, 255, 255, (int)(.75d * 255));
					g.FillColor = new RGBAColor(127, 78, 194, (int)(.75d * 255));
					g.LineWidth = 4;

					g.Polygon(points);

					points.Clear();

					x = new double[] { 15.50, 74.00, 13.87, 75.40, 9.73, 76.06, 2.46, 73.03, 0.00, 69.33, 0.00, 26.67, 2.46, 22.97, 9.73, 19.95, 13.87, 20.60, 15.50, 22.00, 12.00, 23.80, 12.00, 72.20, 15.50, 74.00 };

					for (int i = 0; i < x.Length; i++)
					{
						points.Add(Tuple.Create(Convert.ToSingle(x[i]), Convert.ToSingle(x[++i])));

					}

					g.Polygon(points);



					points.Clear();

					x = new double[] { 96.00, 15.77, 96.00, 16.00, 89.81, 13.07, 15.50, 74.00, 13.87, 75.40, 9.73, 76.06, 2.46, 73.03, 0.00, 69.33, 0.00, 69.00, 4.02, 70.55, 66.00, 2.00, 72.82, 0.84, 92.60, 10.36, 96.00, 15.77 };

					for (int i = 0; i < x.Length; i++)
					{
						points.Add(Tuple.Create(Convert.ToSingle(x[i]), Convert.ToSingle(x[++i])));

					}

					g.Polygon(points);


					points.Clear();

					x = new double[] { 96.00, 80.00, 96.00, 80.23, 92.60, 85.64, 72.82, 95.16, 66.00, 94.00, 4.02, 25.45, 0.00, 27.00, 0.00, 26.67, 2.46, 22.97, 9.73, 19.95, 13.87, 20.60, 15.50, 22.00, 89.81, 82.93, 96.00, 80.00 };

					for (int i = 0; i < x.Length; i++)
					{
						points.Add(Tuple.Create(Convert.ToSingle(x[i]), Convert.ToSingle(x[++i])));

					}

					g.Polygon(points);



					points.Clear();

					x = new double[] { 66.00, 94.00, 66.44, 94.38, 66.00, 94.00 };

					for (int i = 0; i < x.Length; i++)
					{
						points.Add(Tuple.Create(Convert.ToSingle(x[i]), Convert.ToSingle(x[++i])));

					}

					g.Polygon(points);

					//
					g.Ellipse(200, 50, 50, 50);

					g.StateRestore();
				}

				return true;
			}
			return false;
		}
	}
}
