using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
	public static partial class SciterGraphics
	{
		internal static class UnsafeNativeMethods
		{
			public static ISciterGraphicsApi GetApiInterface(ISciterApi sciterApi)
			{
				var sciterGraphicsApi = sciterApi.GetSciterGraphicsAPI();
				
				return new NativeSciterGraphicsApiWrapper<SciterGraphicsApi>(sciterGraphicsApi);
			}

			private sealed class NativeSciterGraphicsApiWrapper<TStruct> : ISciterGraphicsApi
				where TStruct : struct
			{
				private IntPtr _apiPtr;

#pragma warning disable 649

				private readonly SciterGraphicsApiDelegates.ImageCreate _imageCreate;
				private readonly SciterGraphicsApiDelegates.ImageCreateFromPixmap _imageCreateFromPixmap;

				private readonly SciterGraphicsApiDelegates.ImageAddRef _imageAddRef;
				private readonly SciterGraphicsApiDelegates.ImageRelease _imageRelease;
				private readonly SciterGraphicsApiDelegates.ImageGetInfo _imageGetInfo;
				private readonly SciterGraphicsApiDelegates.ImageClear _imageClear;
				private readonly SciterGraphicsApiDelegates.ImageLoad _imageLoad;
				private readonly SciterGraphicsApiDelegates.ImageSave _imageSave;

				private readonly SciterGraphicsApiDelegates.Rgba _rgba;

				private readonly SciterGraphicsApiDelegates.GraphicsCreate _graphicsCreate;
				private readonly SciterGraphicsApiDelegates.GraphicsAddRef _graphicsAddRef;
				private readonly SciterGraphicsApiDelegates.GraphicsRelease _graphicsRelease;

				private readonly SciterGraphicsApiDelegates.GraphicsLine _graphicsLine;
				private readonly SciterGraphicsApiDelegates.GraphicsRectangle _graphicsRectangle;
				private readonly SciterGraphicsApiDelegates.GraphicsRoundedRectangle _graphicsRoundedRectangle;
				private readonly SciterGraphicsApiDelegates.GraphicsEllipse _graphicsEllipse;
				private readonly SciterGraphicsApiDelegates.GraphicsArc _graphicsArc;
				private readonly SciterGraphicsApiDelegates.GraphicsStar _graphicsStar;
				private readonly SciterGraphicsApiDelegates.GraphicsPolygon _graphicsPolygon;
				private readonly SciterGraphicsApiDelegates.GraphicsPolyline _graphicsPolyline;

				private readonly SciterGraphicsApiDelegates.PathCreate _pathCreate;
				private readonly SciterGraphicsApiDelegates.PathAddRef _pathAddRef;
				private readonly SciterGraphicsApiDelegates.PathRelease _pathRelease;
				private readonly SciterGraphicsApiDelegates.PathMoveTo _pathMoveTo;
				private readonly SciterGraphicsApiDelegates.PathLineTo _pathLineTo;
				private readonly SciterGraphicsApiDelegates.PathArcTo _pathArcTo;
				private readonly SciterGraphicsApiDelegates.PathQuadraticCurveTo _pathQuadraticCurveTo;
				private readonly SciterGraphicsApiDelegates.PathBezierCurveTo _pathBezierCurveTo;
				private readonly SciterGraphicsApiDelegates.PathClosePath _pathClosePath;
				private readonly SciterGraphicsApiDelegates.GraphicsDrawPath _graphicsDrawPath;

				private readonly SciterGraphicsApiDelegates.GraphicsRotate _graphicsRotate;
				private readonly SciterGraphicsApiDelegates.GraphicsTranslate _graphicsTranslate;
				private readonly SciterGraphicsApiDelegates.GraphicsScale _graphicsScale;
				private readonly SciterGraphicsApiDelegates.GraphicsSkew _graphicsSkew;
				private readonly SciterGraphicsApiDelegates.GraphicsTransform _graphicsTransform;

				private readonly SciterGraphicsApiDelegates.GraphicsStateSave _graphicsStateSave;
				private readonly SciterGraphicsApiDelegates.GraphicsStateRestore _graphicsStateRestore;

				private readonly SciterGraphicsApiDelegates.GraphicsLineWidth _graphicsLineWidth;
				private readonly SciterGraphicsApiDelegates.GraphicsLineJoin _graphicsLineJoin;
				private readonly SciterGraphicsApiDelegates.GraphicsLineCap _graphicsLineCap;
				private readonly SciterGraphicsApiDelegates.GraphicsLineColor _graphicsLineColor;
				private readonly SciterGraphicsApiDelegates.GraphicsFillColor _graphicsFillColor;
				private readonly SciterGraphicsApiDelegates.GraphicsLineGradientLinear _graphicsLineGradientLinear;
				private readonly SciterGraphicsApiDelegates.GraphicsFillGradientLinear _graphicsFillGradientLinear;
				private readonly SciterGraphicsApiDelegates.GraphicsLineGradientRadial _graphicsLineGradientRadial;
				private readonly SciterGraphicsApiDelegates.GraphicsFillGradientRadial _graphicsFillGradientRadial;
				private readonly SciterGraphicsApiDelegates.GraphicsFillMode _graphicsFillMode;

				private readonly SciterGraphicsApiDelegates.TextCreateForElement _textCreateForElement;
				private readonly SciterGraphicsApiDelegates.TextCreateForElementAndStyle _textCreateForElementAndStyle;
				private readonly SciterGraphicsApiDelegates.TextAddRef _textAddRef;
				private readonly SciterGraphicsApiDelegates.TextRelease _textRelease;
				private readonly SciterGraphicsApiDelegates.TextGetMetrics _textGetMetrics;
				private readonly SciterGraphicsApiDelegates.TextSetBox _textSetBox;
				private readonly SciterGraphicsApiDelegates.GraphicsDrawText _graphicsDrawText;

				private readonly SciterGraphicsApiDelegates.GraphicsDrawImage _graphicsDrawImage;

				private readonly SciterGraphicsApiDelegates.GraphicsWorldToScreen _graphicsWorldToScreen;
				private readonly SciterGraphicsApiDelegates.GraphicsScreenToWorld _graphicsScreenToWorld;

				private readonly SciterGraphicsApiDelegates.GraphicsPushClipBox _graphicsPushClipBox;
				private readonly SciterGraphicsApiDelegates.GraphicsPushClipPath _graphicsPushClipPath;
				private readonly SciterGraphicsApiDelegates.GraphicsPopClip _graphicsPopClip;

				private readonly SciterGraphicsApiDelegates.ImagePaint _imagePaint;

				private readonly SciterGraphicsApiDelegates.ValueWrapGfx _valueWrapGfx;
				private readonly SciterGraphicsApiDelegates.ValueWrapImage _valueWrapImage;
				private readonly SciterGraphicsApiDelegates.ValueWrapPath _valueWrapPath;
				private readonly SciterGraphicsApiDelegates.ValueWrapText _valueWrapText;
				private readonly SciterGraphicsApiDelegates.ValueUnWrapGfx _valueUnWrapGfx;
				private readonly SciterGraphicsApiDelegates.ValueUnWrapImage _valueUnWrapImage;
				private readonly SciterGraphicsApiDelegates.ValueUnWrapPath _valueUnWrapPath;
				private readonly SciterGraphicsApiDelegates.ValueUnWrapText _valueUnWrapText;

#pragma warning restore 649

				internal NativeSciterGraphicsApiWrapper(IntPtr apiPtr)
				{
					_apiPtr = apiPtr;
					var @struct = Marshal.PtrToStructure<TStruct>(apiPtr);

					var fieldInfoDictionary = GetType()
						.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
						.Where(w => w.FieldType.GetCustomAttribute<SciterStructMapAttribute>() != null)
						.ToDictionary(key => key.FieldType.GetCustomAttribute<SciterStructMapAttribute>()?.Name,
							value => value);

					var fieldInfos = @struct.GetType().GetFields();
					foreach (var fieldInfo in fieldInfos)
					{
						if (!fieldInfoDictionary.ContainsKey(fieldInfo.Name))
							continue;
						fieldInfoDictionary[fieldInfo.Name].SetValue(this, fieldInfo.GetValue(@struct));
					}
				}

				public GRAPHIN_RESULT ImageCreate(out IntPtr poutImg, uint width, uint height, bool withAlpha) =>
					_imageCreate(out poutImg, width, height, withAlpha);

				public GRAPHIN_RESULT ImageCreateFromPixmap(out IntPtr poutImg, uint pixmapWidth, uint pixmapHeight,
					bool withAlpha,
					IntPtr pixmap) =>
					_imageCreateFromPixmap(out poutImg, pixmapWidth, pixmapHeight, withAlpha, pixmap);

				public GRAPHIN_RESULT ImageAddRef(IntPtr himg) =>
					_imageAddRef(himg);

				public GRAPHIN_RESULT ImageRelease(IntPtr himg) =>
					_imageRelease(himg);

				public GRAPHIN_RESULT ImageGetInfo(IntPtr himg, out uint width, out uint height, out bool usesAlpha) =>
					_imageGetInfo(himg, out width, out height, out usesAlpha);

				public GRAPHIN_RESULT ImageClear(IntPtr himg, uint byColor) =>
					_imageClear(himg, byColor);

				public GRAPHIN_RESULT ImageLoad(byte[] bytes, uint numBytes, out IntPtr poutImg) =>
					_imageLoad(bytes, numBytes, out poutImg);

				public GRAPHIN_RESULT ImageSave(IntPtr himg, PublicDelegates.ImageWriteFunction pfn, IntPtr prm,
					SCITER_IMAGE_ENCODING bpp, uint quality) =>
					_imageSave(himg, pfn, prm, bpp, quality);

				public uint RGBA(uint red, uint green, uint blue, uint alpha) =>
					_rgba(red, green, blue, alpha);

				public GRAPHIN_RESULT GraphicsCreate(IntPtr img, out IntPtr poutGfx) =>
					_graphicsCreate(img, out poutGfx);

				public GRAPHIN_RESULT GraphicsAddRef(IntPtr hgfx) =>
					_graphicsAddRef(hgfx);

				public GRAPHIN_RESULT GraphicsRelease(IntPtr hgfx) =>
					_graphicsRelease(hgfx);

				public GRAPHIN_RESULT GraphicsLine(IntPtr hgfx, float x1, float y1, float x2, float y2) =>
					_graphicsLine(hgfx, x1, y1, x2, y2);

				public GRAPHIN_RESULT GraphicsRectangle(IntPtr hgfx, float x1, float y1, float x2, float y2) =>
					_graphicsRectangle(hgfx, x1, y1, x2, y2);

				public GRAPHIN_RESULT GraphicsRoundedRectangle(IntPtr hgfx, float x1, float y1, float x2, float y2,
					float[] radii8) =>
					_graphicsRoundedRectangle(hgfx, x1, y1, x2, y2, radii8);

				public GRAPHIN_RESULT GraphicsEllipse(IntPtr hgfx, float x, float y, float rx, float ry) =>
					_graphicsEllipse(hgfx, x, y, rx, ry);

				public GRAPHIN_RESULT GraphicsArc(IntPtr hgfx, float x, float y, float rx, float ry, float start,
					float sweep) =>
					_graphicsArc(hgfx, x, y, rx, ry, start, sweep);

				public GRAPHIN_RESULT GraphicsStar(IntPtr hgfx, float x, float y, float r1, float r2, float start,
					uint rays) =>
					_graphicsStar(hgfx, x, y, r1, r2, start, rays);

				public GRAPHIN_RESULT GraphicsPolygon(IntPtr hgfx, float[] xy, uint numPoints) =>
					_graphicsPolygon(hgfx, xy, numPoints);

				public GRAPHIN_RESULT GraphicsPolyline(IntPtr hgfx, float[] xy, uint numPoints) =>
					_graphicsPolyline(hgfx, xy, numPoints);

				public GRAPHIN_RESULT PathCreate(out IntPtr path) =>
					_pathCreate(out path);

				public GRAPHIN_RESULT PathAddRef(IntPtr path) =>
					_pathAddRef(path);

				public GRAPHIN_RESULT PathRelease(IntPtr gfx) =>
					_pathRelease(gfx);

				public GRAPHIN_RESULT PathMoveTo(IntPtr path, float x, float y, bool relative) =>
					_pathMoveTo(path, x, y, relative);

				public GRAPHIN_RESULT PathLineTo(IntPtr path, float x, float y, bool relative) =>
					_pathLineTo(path, x, y, relative);

				public GRAPHIN_RESULT PathArcTo(IntPtr path, float x, float y, float angle, float rx, float ry,
					bool isLargeArc,
					bool clockwise, bool relative) =>
					_pathArcTo(path, x, y, angle, rx, ry, isLargeArc, clockwise, relative);

				public GRAPHIN_RESULT PathQuadraticCurveTo(IntPtr path, float xc, float yc, float x, float y,
					bool relative) =>
					_pathQuadraticCurveTo(path, xc, yc, x, y, relative);

				public GRAPHIN_RESULT PathBezierCurveTo(IntPtr path, float xc1, float yc1, float xc2, float yc2,
					float x, float y,
					bool relative) =>
					_pathBezierCurveTo(path, xc1, yc1, xc2, yc2, x, y, relative);

				public GRAPHIN_RESULT PathClosePath(IntPtr path) =>
					_pathClosePath(path);

				public GRAPHIN_RESULT GraphicsDrawPath(IntPtr gfx, IntPtr path, DRAW_PATH_MODE dpm) =>
					_graphicsDrawPath(gfx, path, dpm);

				public GRAPHIN_RESULT GraphicsRotate(IntPtr hgfx, float radians, ref float cx, ref float cy) =>
					_graphicsRotate(hgfx, radians, ref cx, ref cy);

				public GRAPHIN_RESULT GraphicsTranslate(IntPtr hgfx, float cx, float cy) =>
					_graphicsTranslate(hgfx, cx, cy);

				public GRAPHIN_RESULT GraphicsScale(IntPtr hgfx, float x, float y) =>
					_graphicsScale(hgfx, x, y);

				public GRAPHIN_RESULT GraphicsSkew(IntPtr hgfx, float dx, float dy) =>
					_graphicsSkew(hgfx, dx, dy);

				public GRAPHIN_RESULT GraphicsTransform(IntPtr hgfx, float m11, float m12, float m21, float m22,
					float dx, float dy) =>
					_graphicsTransform(hgfx, m11, m12, m21, m22, dx, dy);

				public GRAPHIN_RESULT GraphicsStateSave(IntPtr gfx) =>
					_graphicsStateSave(gfx);

				public GRAPHIN_RESULT GraphicsStateRestore(IntPtr gfx) =>
					_graphicsStateRestore(gfx);

				public GRAPHIN_RESULT GraphicsLineWidth(IntPtr hgfx, float width) =>
					_graphicsLineWidth(hgfx, width);

				public GRAPHIN_RESULT GraphicsLineJoin(IntPtr hgfx, SCITER_LINE_JOIN_TYPE lineJoinType) =>
					_graphicsLineJoin(hgfx, lineJoinType);

				public GRAPHIN_RESULT GraphicsLineCap(IntPtr hgfx, SCITER_LINE_CAP_TYPE lineCapType) =>
					_graphicsLineCap(hgfx, lineCapType);

				public GRAPHIN_RESULT GraphicsLineColor(IntPtr gfx, uint color) =>
					_graphicsLineColor(gfx, color);

				public GRAPHIN_RESULT GraphicsFillColor(IntPtr gfx, uint color) =>
					_graphicsFillColor(gfx, color);

				public GRAPHIN_RESULT GraphicsLineGradientLinear(IntPtr hgfx, float x1, float y1, float x2, float y2,
					COLOR_STOP[] stops, uint nstops) =>
					_graphicsLineGradientLinear(hgfx, x1, y1, x2, y2, stops, nstops);

				public GRAPHIN_RESULT GraphicsFillGradientLinear(IntPtr hgfx, float x1, float y1, float x2, float y2,
					COLOR_STOP[] stops, uint nstops) =>
					_graphicsFillGradientLinear(hgfx, x1, y1, x2, y2, stops, nstops);

				public GRAPHIN_RESULT GraphicsLineGradientRadial(IntPtr hgfx, float x, float y, float rx, float ry,
					COLOR_STOP[] stops, uint nstops) =>
					_graphicsLineGradientRadial(hgfx, x, y, rx, ry, stops, nstops);

				public GRAPHIN_RESULT GraphicsFillGradientRadial(IntPtr hgfx, float x, float y, float rx, float ry,
					COLOR_STOP[] stops, uint nstops) =>
					_graphicsFillGradientRadial(hgfx, x, y, rx, ry, stops, nstops);

				public GRAPHIN_RESULT GraphicsFillMode(IntPtr hgfx, bool evenOdd) =>
					_graphicsFillMode(hgfx, evenOdd);

				public GRAPHIN_RESULT TextCreateForElement(out IntPtr ptext, string text, uint textLength, IntPtr he,
					string classNameOrNull) =>
					_textCreateForElement(out ptext, text, textLength, he, classNameOrNull);

				public GRAPHIN_RESULT TextCreateForElementAndStyle(out IntPtr ptext, string text, uint textLength,
					IntPtr he, string style, uint styleLength) =>
					_textCreateForElementAndStyle(out ptext, text, textLength, he, style, styleLength);

				public GRAPHIN_RESULT TextAddRef(IntPtr htext) =>
					_textAddRef(htext);

				public GRAPHIN_RESULT TextRelease(IntPtr htext) =>
					_textRelease(htext);

				public GRAPHIN_RESULT TextGetMetrics(IntPtr htext, out float minWidth, out float maxWidth,
					out float height, out float ascent, out float descent, out uint nLines) =>
					_textGetMetrics(htext, out minWidth, out maxWidth, out height, out ascent, out descent, out nLines);

				public GRAPHIN_RESULT TextSetBox(IntPtr htext, float width, float height) =>
					_textSetBox(htext, width, height);

				public GRAPHIN_RESULT GraphicsDrawText(IntPtr hgfx, IntPtr text, float px, float py, uint position) =>
					_graphicsDrawText(hgfx, text, px, py, position);

				public GRAPHIN_RESULT GraphicsDrawImage(IntPtr hgfx, IntPtr himg, float x, float y, IntPtr w, IntPtr h,
					IntPtr ix, IntPtr iy, IntPtr iw, IntPtr ih, IntPtr opacity) =>
					_graphicsDrawImage(hgfx, himg, x, y, w, h, ix, iy, iw, ih, opacity);

				public GRAPHIN_RESULT GraphicsWorldToScreen(IntPtr hgfx, ref float inoutX, ref float inoutY) =>
					_graphicsWorldToScreen(hgfx, ref inoutX, ref inoutY);

				public GRAPHIN_RESULT GraphicsScreenToWorld(IntPtr hgfx, ref float inoutX, ref float inoutY) =>
					_graphicsScreenToWorld(hgfx, ref inoutX, ref inoutY);

				public GRAPHIN_RESULT GraphicsPushClipBox(IntPtr hgfx, float x1, float y1, float x2, float y2,
					float opacity = 1f) =>
					_graphicsPushClipBox(hgfx, x1, y1, x2, y2, opacity);

				public GRAPHIN_RESULT GraphicsPushClipPath(IntPtr hgfx, IntPtr hpath, float opacity = 1f) =>
					_graphicsPushClipPath(hgfx, hpath, opacity);

				public GRAPHIN_RESULT GraphicsPopClip(IntPtr hgfx) =>
					_graphicsPopClip(hgfx);

				public GRAPHIN_RESULT ImagePaint(IntPtr himg, PublicDelegates.ImagePaintFunction pPainter,
					IntPtr prm) =>
					_imagePaint(himg, pPainter, prm);

				public GRAPHIN_RESULT ValueWrapGfx(IntPtr hgfx, out SciterValue.VALUE toValue) =>
					_valueWrapGfx(hgfx, out toValue);

				public GRAPHIN_RESULT ValueWrapImage(IntPtr himg, out SciterValue.VALUE toValue) =>
					_valueWrapImage(himg, out toValue);

				public GRAPHIN_RESULT ValueWrapPath(IntPtr hpath, out SciterValue.VALUE toValue) =>
					_valueWrapPath(hpath, out toValue);

				public GRAPHIN_RESULT ValueWrapText(IntPtr htext, out SciterValue.VALUE toValue) =>
					_valueWrapText(htext, out toValue);

				public GRAPHIN_RESULT ValueUnWrapGfx(ref SciterValue.VALUE fromValue, out IntPtr phgfx) =>
					_valueUnWrapGfx(ref fromValue, out phgfx);

				public GRAPHIN_RESULT ValueUnWrapImage(ref SciterValue.VALUE fromValue, out IntPtr phimg) =>
					_valueUnWrapImage(ref fromValue, out phimg);

				public GRAPHIN_RESULT ValueUnWrapPath(ref SciterValue.VALUE fromValue, out IntPtr phpath) =>
					_valueUnWrapPath(ref fromValue, out phpath);

				public GRAPHIN_RESULT ValueUnWrapText(ref SciterValue.VALUE fromValue, out IntPtr phtext) =>
					_valueUnWrapText(ref fromValue, out phtext);
			}
		}
	}
}