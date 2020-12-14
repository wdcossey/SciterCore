using System;
using System.Runtime.InteropServices;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
    public interface ISciterGraphicsApi
    {
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="poutImg"></param>
	    /// <param name="width"></param>
	    /// <param name="height"></param>
	    /// <param name="withAlpha"></param>
	    /// <returns></returns>
	    SciterGraphics.GRAPHIN_RESULT ImageCreate(out IntPtr poutImg, uint width, uint height, bool withAlpha);
	    
	    /// <summary>
	    /// Constructs an image from B[n+0],G[n+1],R[n+2],A[n+3] data.
	    /// </summary>
	    /// <param name="poutImg"></param>
	    /// <param name="pixmapWidth"></param>
	    /// <param name="pixmapHeight"></param>
	    /// <param name="withAlpha"></param>
	    /// <param name="pixmap"></param>
	    /// <returns></returns>
	    SciterGraphics.GRAPHIN_RESULT ImageCreateFromPixmap(out IntPtr poutImg, uint pixmapWidth, uint pixmapHeight, bool withAlpha, IntPtr pixmap);
	    
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="himg"></param>
	    /// <returns></returns>
	    SciterGraphics.GRAPHIN_RESULT ImageAddRef(IntPtr himg);
	    
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="himg"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ImageRelease(IntPtr himg);
		
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="himg"></param>
	    /// <param name="width"></param>
	    /// <param name="height"></param>
	    /// <param name="usesAlpha"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ImageGetInfo(IntPtr himg, out uint width, out uint height, out bool usesAlpha);
		
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="himg"></param>
	    /// <param name="byColor"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ImageClear(IntPtr himg, uint byColor);
		
	    /// <summary>
	    /// Load png/jpeg/etc. image from stream of bytes
	    /// </summary>
	    /// <param name="bytes"></param>
	    /// <param name="numBytes"></param>
	    /// <param name="pout_img"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ImageLoad(byte[] bytes, uint numBytes, out IntPtr pout_img);
	    
	    /// <summary>
	    /// Save png/jpeg/etc. image to stream of bytes
	    /// </summary>
	    /// <param name="himg"></param>
	    /// <param name="pfn"></param>
	    /// <param name="prm"></param>
	    /// <param name="bpp"></param>
	    /// <param name="quality"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ImageSave(IntPtr himg, SciterGraphics.PublicDelegates.ImageWriteFunction pfn, IntPtr prm, SciterGraphics.SCITER_IMAGE_ENCODING bpp, uint quality);

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="red"></param>
	    /// <param name="green"></param>
	    /// <param name="blue"></param>
	    /// <param name="alpha"></param>
	    /// <returns></returns>
		uint RGBA(uint red, uint green, uint blue, uint alpha);

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="img"></param>
	    /// <param name="poutGfx"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsCreate(IntPtr img, out IntPtr poutGfx);
	    
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsAddRef(IntPtr hgfx);
	    
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsRelease(IntPtr hgfx);
	    
	    /// <summary>
	    /// Draws line from x1,y1 to x2,y2 using current lineColor and lineGradient.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="x1"></param>
	    /// <param name="y1"></param>
	    /// <param name="x2"></param>
	    /// <param name="y2"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsLine(IntPtr hgfx, float x1, float y1, float x2, float y2);
	    
	    /// <summary>
	    /// Draws rectangle using current lineColor/lineGradient and fillColor/fillGradient with (optional) rounded corners.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="x1"></param>
	    /// <param name="y1"></param>
	    /// <param name="x2"></param>
	    /// <param name="y2"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsRectangle(IntPtr hgfx, float x1, float y1, float x2, float y2);
	    
	    /// <summary>
	    /// Draws rounded rectangle using current lineColor/lineGradient and fillColor/fillGradient with (optional) rounded corners.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="x1"></param>
	    /// <param name="y1"></param>
	    /// <param name="x2"></param>
	    /// <param name="y2"></param>
	    /// <param name="radii8"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsRoundedRectangle(IntPtr hgfx, float x1, float y1, float x2, float y2, float[] radii8);
	    
	    /// <summary>
	    /// Draws circle or ellipse using current lineColor/lineGradient and fillColor/fillGradient.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="x"></param>
	    /// <param name="y"></param>
	    /// <param name="rx"></param>
	    /// <param name="ry"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsEllipse(IntPtr hgfx, float x, float y, float rx, float ry);
	    
	    /// <summary>
	    /// Draws closed arc using current lineColor/lineGradient and fillColor/fillGradient.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="x"></param>
	    /// <param name="y"></param>
	    /// <param name="rx"></param>
	    /// <param name="ry"></param>
	    /// <param name="start"></param>
	    /// <param name="sweep"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsArc(IntPtr hgfx, float x, float y, float rx, float ry, float start, float sweep);
	    
	    /// <summary>
	    /// Draws star.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="x"></param>
	    /// <param name="y"></param>
	    /// <param name="r1"></param>
	    /// <param name="r2"></param>
	    /// <param name="start"></param>
	    /// <param name="rays"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsStar(IntPtr hgfx, float x, float y, float r1, float r2, float start, uint rays);
	    
	    /// <summary>
	    /// Closed polygon.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="xy"></param>
	    /// <param name="numPoints"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsPolygon(IntPtr hgfx, float[] xy, uint numPoints);
	    
	    /// <summary>
	    /// Polyline.
	    /// </summary>
	    /// <param name="hgfx"></param>
	    /// <param name="xy"></param>
	    /// <param name="numPoints"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsPolyline(IntPtr hgfx, float[] xy, uint numPoints);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathCreate(out IntPtr path);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathAddRef(IntPtr path);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gfx"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathRelease(IntPtr gfx);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="relative"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathMoveTo(IntPtr path, float x, float y, bool relative);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="relative"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathLineTo(IntPtr path, float x, float y, bool relative);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="angle"></param>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		/// <param name="isLargeArc"></param>
		/// <param name="clockwise"></param>
		/// <param name="relative"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathArcTo(IntPtr path, float x, float y, float angle, float rx, float ry, bool isLargeArc, bool clockwise, bool relative);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="xc"></param>
		/// <param name="yc"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="relative"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathQuadraticCurveTo(IntPtr path, float xc, float yc, float x, float y, bool relative);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="xc1"></param>
		/// <param name="yc1"></param>
		/// <param name="xc2"></param>
		/// <param name="yc2"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="relative"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathBezierCurveTo(IntPtr path, float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT PathClosePath(IntPtr path);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gfx"></param>
		/// <param name="path"></param>
		/// <param name="dpm"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsDrawPath(IntPtr gfx, IntPtr path, SciterGraphics.DRAW_PATH_MODE dpm);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="radians"></param>
		/// <param name="cx"></param>
		/// <param name="cy"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsRotate(IntPtr hgfx, float radians, ref float cx, ref float cy);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="cx"></param>
		/// <param name="cy"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsTranslate(IntPtr hgfx, float cx, float cy);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsScale(IntPtr hgfx, float x, float y);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsSkew(IntPtr hgfx, float dx, float dy);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="m11"></param>
		/// <param name="m12"></param>
		/// <param name="m21"></param>
		/// <param name="m22"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsTransform(IntPtr hgfx, float m11, float m12, float m21, float m22, float dx, float dy);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gfx"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsStateSave(IntPtr gfx);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="gfx"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsStateRestore(IntPtr gfx);
		
		/// <summary>
		/// Sets line width for subsequent drawings.
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsLineWidth(IntPtr hgfx, float width);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="lineJoinType"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsLineJoin(IntPtr hgfx, SciterGraphics.SCITER_LINE_JOIN_TYPE lineJoinType);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="lineCapType"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsLineCap(IntPtr hgfx, SciterGraphics.SCITER_LINE_CAP_TYPE lineCapType);
		
		/// <summary>
		/// COLOR for solid lines/strokes
		/// </summary>
		/// <param name="gfx"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsLineColor(IntPtr gfx, uint color);
		
		/// <summary>
		/// COLOR for solid fills
		/// </summary>
		/// <param name="gfx"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsFillColor(IntPtr gfx, uint color);
		
		/// <summary>
		/// Setup parameters of linear gradient of lines.
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="stops"></param>
		/// <param name="nstops"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsLineGradientLinear(IntPtr hgfx, float x1, float y1, float x2, float y2, SciterGraphics.COLOR_STOP[] stops, uint nstops);
		
		/// <summary>
		/// Setup parameters of linear gradient of fills.
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="stops"></param>
		/// <param name="nstops"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsFillGradientLinear(IntPtr hgfx, float x1, float y1, float x2, float y2, SciterGraphics.COLOR_STOP[] stops, uint nstops);
		
		/// <summary>
		/// Setup parameters of line gradient radial fills.
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		/// <param name="stops"></param>
		/// <param name="nstops"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsLineGradientRadial(IntPtr hgfx, float x, float y, float rx, float ry, SciterGraphics.COLOR_STOP[] stops, uint nstops);
		
		/// <summary>
		/// Setup parameters of gradient radial fills.
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		/// <param name="stops"></param>
		/// <param name="nstops"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsFillGradientRadial(IntPtr hgfx, float x, float y, float rx, float ry, SciterGraphics.COLOR_STOP[] stops, uint nstops);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="even_odd"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsFillMode(IntPtr hgfx, bool even_odd);
		
		
		/// <summary>
		/// Create text layout using element's styles
		/// </summary>
		/// <param name="ptext"></param>
		/// <param name="text"></param>
		/// <param name="textLength"></param>
		/// <param name="he"></param>
		/// <param name="classNameOrNull"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT TextCreateForElement(out IntPtr ptext, string text, uint textLength, IntPtr he, string classNameOrNull);
		
		/// <summary>
		/// Create text layout using explicit format declaration
		/// </summary>
		/// <param name="ptext"></param>
		/// <param name="text"></param>
		/// <param name="textLength"></param>
		/// <param name="he"></param>
		/// <param name="style"></param>
		/// <param name="styleLength"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT TextCreateForElementAndStyle(out IntPtr ptext, string text, uint textLength, IntPtr he, string style, uint styleLength);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="htext"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT TextAddRef(IntPtr htext);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="htext"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT TextRelease(IntPtr htext);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="htext"></param>
		/// <param name="minWidth"></param>
		/// <param name="maxWidth"></param>
		/// <param name="height"></param>
		/// <param name="ascent"></param>
		/// <param name="descent"></param>
		/// <param name="nLines"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT TextGetMetrics(IntPtr htext, out float minWidth, out float maxWidth, out float height, out float ascent, out float descent, out uint nLines);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="htext"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT TextSetBox(IntPtr htext, float width, float height);
		
		
		/// <summary>
		/// Draw text with position (1..9 on MUMPAD) at px,py <br/>
		/// Ex: gDrawText( 100,100,5) will draw text box with its center at 100,100 px 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="text"></param>
		/// <param name="px"></param>
		/// <param name="py"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsDrawText(IntPtr hgfx, IntPtr text, float px, float py, uint position);
		
		/// <summary>
		/// Draws img onto the graphics surface with current transformation applied (scale, rotation).
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="himg"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="ix"></param>
		/// <param name="iy"></param>
		/// <param name="iw"></param>
		/// <param name="ih"></param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsDrawImage(IntPtr hgfx, IntPtr himg, float x, float y, IntPtr w, IntPtr h, IntPtr ix, IntPtr iy, IntPtr iw, IntPtr ih, IntPtr opacity);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="inoutX"></param>
		/// <param name="inoutY"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsWorldToScreen(IntPtr hgfx, ref float inoutX, ref float inoutY);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="inoutX"></param>
		/// <param name="inoutY"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsScreenToWorld(IntPtr hgfx, ref float inoutX, ref float inoutY);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsPushClipBox(IntPtr hgfx, float x1, float y1, float x2, float y2, float opacity = 1f);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="hpath"></param>
		/// <param name="opacity"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsPushClipPath(IntPtr hgfx, IntPtr hpath, float opacity =1f);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT GraphicsPopClip(IntPtr hgfx);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="himg"></param>
		/// <param name="pPainter"></param>
		/// <param name="prm"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ImagePaint(IntPtr himg, SciterGraphics.PublicDelegates.ImagePaintFunction pPainter, IntPtr prm);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="hgfx"></param>
		/// <param name="toValue"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueWrapGfx(IntPtr hgfx, out SciterValue.VALUE toValue);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="himg"></param>
		/// <param name="toValue"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueWrapImage(IntPtr himg, out SciterValue.VALUE toValue);
		
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="hpath"></param>
	    /// <param name="toValue"></param>
	    /// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueWrapPath(IntPtr hpath, out SciterValue.VALUE toValue);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="htext"></param>
		/// <param name="toValue"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueWrapText(IntPtr htext, out SciterValue.VALUE toValue);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fromValue"></param>
		/// <param name="phgfx"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueUnWrapGfx(ref SciterValue.VALUE fromValue, out IntPtr phgfx);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fromValue"></param>
		/// <param name="phimg"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueUnWrapImage(ref SciterValue.VALUE fromValue, out IntPtr phimg);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fromValue"></param>
		/// <param name="phpath"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueUnWrapPath(ref SciterValue.VALUE fromValue, out IntPtr phpath);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fromValue"></param>
		/// <param name="phtext"></param>
		/// <returns></returns>
		SciterGraphics.GRAPHIN_RESULT ValueUnWrapText(ref SciterValue.VALUE fromValue, out IntPtr phtext);
			
    }
}