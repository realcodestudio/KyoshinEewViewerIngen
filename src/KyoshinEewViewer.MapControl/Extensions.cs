﻿using KyoshinMonitorLib;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace KyoshinEewViewer.MapControl
{
	public static class Extensions
	{
		public static Point ToPixel(this Location loc, double zoom)
			=> MercatorProjection.LatLngToPixel(loc, zoom);
		public static Location ToLocation(this Point loc, double zoom)
			=> MercatorProjection.PixelToLatLng(loc, zoom);

		public static Point AsPoint(this Location loc)
			=> new Point(loc.Latitude, loc.Longitude);
		public static Location AsLocation(this Point loc)
			=> new Location((float)loc.X, (float)loc.Y);

		public static Location[] ToLocations(this IntVector[] points, TopologyMap map)
		{
			var result = new Location[points.Length];
			double x = 0;
			double y = 0;
			for (var i = 0; i < result.Length; i++)
				result[i] = new Location((float)((x += points[i].X) * map.Scale.X + map.Translate.X), (float)((y += points[i].Y) * map.Scale.Y + map.Translate.Y));
			return result;
		}


		private static IEnumerable<PathSegment> ToLineSegments(this IEnumerable<Point> points)
			=> points.Select(pos => new LineSegment(pos, true));
		public static PathFigure ToPolygonPathFigure(this Location[] nodes, double zoom, bool closed = true)
		{
			var points = DouglasPeucker.Reduction(nodes.Select(n => n.ToPixel(zoom)).ToArray(), .9);
			if (closed && points.Length <= 3) // 小さなポリゴンは描画しない
				return null;
			return new PathFigure(points[0], points[1..].ToLineSegments(), closed);
		}
	}
}