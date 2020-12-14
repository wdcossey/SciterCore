using System;
using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
    public static partial class SciterGraphics
    {
	    public static class PublicDelegates
	    {
		    public delegate bool ImageWriteFunction(IntPtr prm, IntPtr data, uint dataLength);
		    
		    public delegate bool ImagePaintFunction(IntPtr prm, IntPtr hgfx, uint width, uint height);
	    }

	    internal static class SciterGraphicsApiDelegates
        {
            
			#region SECTION: image primitives

			/// <summary>
			/// GRAPHIN_RESULT SCFN(imageCreate)( HIMG* poutImg, UINT width, UINT height, BOOL withAlpha );
			/// </summary>
			/// <param name="poutImg"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			/// <param name="withAlpha"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageCreate))]
			
			public delegate GRAPHIN_RESULT ImageCreate(out IntPtr poutImg, uint width, uint height, bool withAlpha);


			/// <summary>
			/// construct image from B[n+0],G[n+1],R[n+2],A[n+3] data. <br/>
			/// Size of pixmap data is pixmapWidth*pixmapHeight*4 <br/>
			/// GRAPHIN_RESULT SCFN(imageCreateFromPixmap)( HIMG* poutImg, UINT pixmapWidth, UINT pixmapHeight, BOOL withAlpha, const BYTE* pixmapPixels );
			/// </summary>
			/// <param name="poutImg"></param>
			/// <param name="pixmapWidth"></param>
			/// <param name="pixmapHeight"></param>
			/// <param name="withAlpha"></param>
			/// <param name="pixmap"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageCreateFromPixmap))]
			public delegate GRAPHIN_RESULT ImageCreateFromPixmap(out IntPtr poutImg, uint pixmapWidth, uint pixmapHeight, bool withAlpha, IntPtr pixmap);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(imageAddRef)( HIMG himg );
			/// </summary>
			/// <param name="himg"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageAddRef))]
			public delegate GRAPHIN_RESULT ImageAddRef(IntPtr himg);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(imageRelease)( HIMG himg );
			/// </summary>
			/// <param name="himg"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageRelease))]
			public delegate GRAPHIN_RESULT ImageRelease(IntPtr himg);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(imageGetInfo)( HIMG himg, UINT* width, UINT* height, BOOL* usesAlpha );
			/// </summary>
			/// <param name="himg"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			/// <param name="usesAlpha"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageGetInfo))]
			public delegate GRAPHIN_RESULT ImageGetInfo(IntPtr himg, out uint width, out uint height, out bool usesAlpha);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(imageClear)( HIMG himg, COLOR byColor );
			/// </summary>
			/// <param name="himg"></param>
			/// <param name="byColor"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageClear))]
			public delegate GRAPHIN_RESULT ImageClear(IntPtr himg, uint byColor);

			/// <summary>
			/// load png/jpeg/etc. image from stream of bytes <br/>
			/// GRAPHIN_RESULT SCFN(imageLoad)( const BYTE* bytes, UINT num_bytes, HIMG* pout_img );
			/// </summary>
			/// <param name="bytes"></param>
			/// <param name="numBytes"></param>
			/// <param name="poutImg"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageLoad))]
			public delegate GRAPHIN_RESULT ImageLoad(byte[] bytes, uint numBytes, out IntPtr poutImg);
			
			/// <summary>
			/// Save png/jpeg/etc. image to stream of bytes <br/>
			/// GRAPHIN_RESULT SCFN(imageSave) ( HIMG himg, image_write_function* pfn,	void* prm, UINT bpp, UINT quality );
			/// </summary>
			/// <param name="himg"></param>
			/// <param name="pfn"></param>
			/// <param name="prm">function and its param passed "as is"</param>
			/// <param name="bpp">24,32 if alpha needed</param>
			/// <param name="quality">png: 0, jpeg:, 10 - 100 </param>
			[SciterStructMap(nameof(SciterGraphicsApi.imageSave))]
			public delegate GRAPHIN_RESULT ImageSave(IntPtr himg, PublicDelegates.ImageWriteFunction pfn, IntPtr prm, SCITER_IMAGE_ENCODING bpp, uint quality);

			#endregion

			#region SECTION: graphics primitives and drawing operations
			
			/// <summary>
			/// COLOR SCFN(RGBA)(UINT red, UINT green, UINT blue, UINT alpha /*= 255*/);
			/// </summary>
			/// <param name="red"></param>
			/// <param name="green"></param>
			/// <param name="blue"></param>
			/// <param name="alpha"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.RGBA))]
			public delegate uint Rgba(uint red, uint green, uint blue, uint alpha);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gCreate)(HIMG img, HGFX* pout_gfx );
			/// </summary>
			/// <param name="img"></param>
			/// <param name="poutGfx"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gCreate))]
			public delegate GRAPHIN_RESULT GraphicsCreate(IntPtr img, out IntPtr poutGfx);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gAddRef) (HGFX hgfx);
			/// </summary>
			/// <param name="hgfx"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gAddRef))]
			public delegate GRAPHIN_RESULT GraphicsAddRef(IntPtr hgfx);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gRelease) (HGFX hgfx)
			/// </summary>
			/// <param name="hgfx"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gRelease))]
			public delegate GRAPHIN_RESULT GraphicsRelease(IntPtr hgfx);

			/// <summary>
			/// Draws line from x1,y1 to x2,y2 using current lineColor and lineGradient. <br/>
			/// GRAPHIN_RESULT SCFN(gLine) ( HGFX hgfx, POS x1, POS y1, POS x2, POS y2 );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x1"></param>
			/// <param name="y1"></param>
			/// <param name="x2"></param>
			/// <param name="y2"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gLine))]
			public delegate GRAPHIN_RESULT GraphicsLine(IntPtr hgfx, float x1, float y1, float x2, float y2);

			/// <summary>
			/// Draws rectangle using current lineColor/lineGradient and fillColor/fillGradient with (optional) rounded corners. <br/>
			/// GRAPHIN_RESULT SCFN(gRectangle) ( HGFX hgfx, POS x1, POS y1, POS x2, POS y2 );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x1"></param>
			/// <param name="y1"></param>
			/// <param name="x2"></param>
			/// <param name="y2"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gRectangle))]
			public delegate GRAPHIN_RESULT GraphicsRectangle(IntPtr hgfx, float x1, float y1, float x2, float y2);

			/// <summary>
			/// Draws rounded rectangle using current lineColor/lineGradient and fillColor/fillGradient with (optional) rounded corners. <br/>
			/// GRAPHIN_RESULT SCFN(gRoundedRectangle) ( HGFX hgfx, POS x1, POS y1, POS x2, POS y2, const DIM* radii8 /*DIM[8] - four rx/ry pairs */);
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x1"></param>
			/// <param name="y1"></param>
			/// <param name="x2"></param>
			/// <param name="y2"></param>
			/// <param name="radii8"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gRoundedRectangle))]
			public delegate GRAPHIN_RESULT GraphicsRoundedRectangle(IntPtr hgfx, float x1, float y1, float x2, float y2, float[] radii8);

			/// <summary>
			/// Draws circle or ellipse using current lineColor/lineGradient and fillColor/fillGradient. <br/>
			/// GRAPHIN_RESULT SCFN(gEllipse) ( HGFX hgfx, POS x, POS y, DIM rx, DIM ry );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="rx"></param>
			/// <param name="ry"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gEllipse))]
			public delegate GRAPHIN_RESULT GraphicsEllipse(IntPtr hgfx, float x, float y, float rx, float ry);

			/// <summary>
			/// Draws closed arc using current lineColor/lineGradient and fillColor/fillGradient. <br/>
			/// GRAPHIN_RESULT SCFN(gArc) ( HGFX hgfx, POS x, POS y, POS rx, POS ry, ANGLE start, ANGLE sweep );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="rx"></param>
			/// <param name="ry"></param>
			/// <param name="start"></param>
			/// <param name="sweep"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gArc))]
			public delegate GRAPHIN_RESULT GraphicsArc(IntPtr hgfx, float x, float y, float rx, float ry, float start, float sweep);

			/// <summary>
			/// Draws star. <br/>
			/// GRAPHIN_RESULT SCFN(gStar) ( HGFX hgfx, POS x, POS y, DIM r1, DIM r2, ANGLE start, UINT rays );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="r1"></param>
			/// <param name="r2"></param>
			/// <param name="start"></param>
			/// <param name="rays"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gStar))]
			public delegate GRAPHIN_RESULT GraphicsStar(IntPtr hgfx, float x, float y, float r1, float r2, float start, uint rays);
			
			/// <summary>
			/// Closed polygon. <br/>
			/// GRAPHIN_RESULT SCFN(gPolygon) ( HGFX hgfx, const POS* xy, UINT num_points );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="xy"></param>
			/// <param name="numPoints"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gPolygon))]
			public delegate GRAPHIN_RESULT GraphicsPolygon(IntPtr hgfx, float[] xy, uint numPoints);

			// Polyline.
			// GRAPHIN_RESULT SCFN(gPolyline) ( HGFX hgfx, const POS* xy, UINT num_points );
			[SciterStructMap(nameof(SciterGraphicsApi.gPolyline))]
			public delegate GRAPHIN_RESULT GraphicsPolyline(IntPtr hgfx, float[] xy, uint numPoints);
			#endregion

			#region SECTION: Path operations

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathCreate) ( HPATH* path );
			/// </summary>
			/// <param name="path"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathCreate))]
			public delegate GRAPHIN_RESULT PathCreate(out IntPtr path);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathAddRef) ( HPATH path );
			/// </summary>
			/// <param name="path"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathAddRef))]
			public delegate GRAPHIN_RESULT PathAddRef(IntPtr path);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathRelease) ( HPATH path );
			/// </summary>
			/// <param name="gfx"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathRelease))]
			public delegate GRAPHIN_RESULT PathRelease(IntPtr gfx);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathMoveTo) ( HPATH path, POS x, POS y, BOOL relative );
			/// </summary>
			/// <param name="path"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="relative"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathMoveTo))]
			public delegate GRAPHIN_RESULT PathMoveTo(IntPtr path, float x, float y, bool relative);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathLineTo) ( HPATH path, POS x, POS y, BOOL relative );
			/// </summary>
			/// <param name="path"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="relative"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathLineTo))]
			public delegate GRAPHIN_RESULT PathLineTo(IntPtr path, float x, float y, bool relative);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathArcTo) ( HPATH path, POS x, POS y, ANGLE angle, DIM rx, DIM ry, BOOL is_large_arc, BOOL clockwise, BOOL relative );
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
			[SciterStructMap(nameof(SciterGraphicsApi.pathArcTo))]
			public delegate GRAPHIN_RESULT PathArcTo(IntPtr path, float x, float y, float angle, float rx, float ry, bool isLargeArc, bool clockwise, bool relative);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathQuadraticCurveTo) ( HPATH path, POS xc, POS yc, POS x, POS y, BOOL relative );
			/// </summary>
			/// <param name="path"></param>
			/// <param name="xc"></param>
			/// <param name="yc"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="relative"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathQuadraticCurveTo))]
			public delegate GRAPHIN_RESULT PathQuadraticCurveTo(IntPtr path, float xc, float yc, float x, float y, bool relative);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathBezierCurveTo) ( HPATH path, POS xc1, POS yc1, POS xc2, POS yc2, POS x, POS y, BOOL relative );
			/// </summary>
			/// <param name="path"></param>
			/// <param name="xc1"></param>
			/// <param name="yc1"></param>
			/// <param name="xc2"></param>
			/// <param name="yc2"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="relative"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathBezierCurveTo))]
			public delegate GRAPHIN_RESULT PathBezierCurveTo(IntPtr path, float xc1, float yc1, float xc2, float yc2, float x, float y, bool relative);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(pathClosePath) ( HPATH path );
			/// </summary>
			/// <param name="path"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.pathClosePath))]
			public delegate GRAPHIN_RESULT PathClosePath(IntPtr path);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gDrawPath) ( HGFX hgfx, HPATH path, DRAW_PATH_MODE dpm );
			/// </summary>
			/// <param name="gfx"></param>
			/// <param name="path"></param>
			/// <param name="dpm"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gDrawPath))]
			public delegate GRAPHIN_RESULT GraphicsDrawPath(IntPtr gfx, IntPtr path, DRAW_PATH_MODE dpm);

			#endregion

			#region SECTION: affine tranformations

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gRotate) ( HGFX hgfx, ANGLE radians, POS* cx /*= 0*/, POS* cy /*= 0*/ );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="radians"></param>
			/// <param name="cx"></param>
			/// <param name="cy"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gRotate))]
			public delegate GRAPHIN_RESULT GraphicsRotate(IntPtr hgfx, float radians, ref float cx, ref float cy);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gTranslate) ( HGFX hgfx, POS cx, POS cy );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="cx"></param>
			/// <param name="cy"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gTranslate))]
			public delegate GRAPHIN_RESULT GraphicsTranslate(IntPtr hgfx, float cx, float cy);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gScale) ( HGFX hgfx, DIM x, DIM y );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gScale))]
			public delegate GRAPHIN_RESULT GraphicsScale(IntPtr hgfx, float x, float y);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gSkew) ( HGFX hgfx, DIM dx, DIM dy );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="dx"></param>
			/// <param name="dy"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gSkew))]
			public delegate GRAPHIN_RESULT GraphicsSkew(IntPtr hgfx, float dx, float dy);
			
			/// <summary>
			/// all above in one shot <br/>
			/// GRAPHIN_RESULT SCFN(gTransform) ( HGFX hgfx, POS m11, POS m12, POS m21, POS m22, POS dx, POS dy );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="m11"></param>
			/// <param name="m12"></param>
			/// <param name="m21"></param>
			/// <param name="m22"></param>
			/// <param name="dx"></param>
			/// <param name="dy"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gTransform))]
			public delegate GRAPHIN_RESULT GraphicsTransform(IntPtr hgfx, float m11, float m12, float m21, float m22, float dx, float dy);

			#endregion

			#region SECTION: state save/restore

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gStateSave) ( HGFX hgfx );
			/// </summary>
			/// <param name="gfx"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gStateSave))]
			public delegate GRAPHIN_RESULT GraphicsStateSave(IntPtr gfx);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gStateRestore) ( HGFX hgfx );
			/// </summary>
			/// <param name="gfx"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gStateRestore))]
			public delegate GRAPHIN_RESULT GraphicsStateRestore(IntPtr gfx);

			#endregion

			#region SECTION: drawing attributes

			/// <summary>
			/// set line width for subsequent drawings. <br/>
			/// GRAPHIN_RESULT SCFN(gLineWidth) ( HGFX hgfx, DIM width );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="width"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gLineWidth))]
			public delegate GRAPHIN_RESULT GraphicsLineWidth(IntPtr hgfx, float width);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gLineJoin) ( HGFX hgfx, SCITER_LINE_JOIN_TYPE type );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="type"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gLineJoin))]
			public delegate GRAPHIN_RESULT GraphicsLineJoin(IntPtr hgfx, SCITER_LINE_JOIN_TYPE type);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gLineCap) ( HGFX hgfx, SCITER_LINE_CAP_TYPE type);
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="type"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gLineCap))]
			public delegate GRAPHIN_RESULT GraphicsLineCap(IntPtr hgfx, SCITER_LINE_CAP_TYPE type);

			//GRAPHIN_RESULT SCFN
			//      (*gNoLine ( HGFX hgfx ) { gLineWidth(hgfx,0.0); }
			
			/// <summary>
			/// COLOR for solid lines/strokes <br/>
			/// GRAPHIN_RESULT SCFN(gLineColor) ( HGFX hgfx, COLOR color);
			/// </summary>
			/// <param name="gfx"></param>
			/// <param name="color"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gLineColor))]
			public delegate GRAPHIN_RESULT GraphicsLineColor(IntPtr gfx, uint color);

			/// <summary>
			/// COLOR for solid fills <br/>
			/// GRAPHIN_RESULT SCFN(gFillColor) ( HGFX hgfx, COLOR color );
			/// </summary>
			/// <param name="gfx"></param>
			/// <param name="color"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gFillColor))]
			public delegate GRAPHIN_RESULT GraphicsFillColor(IntPtr gfx, uint color);

			//inline void
			//      graphics_no_fill ( HGFX hgfx ) { graphics_fill_color(hgfx, graphics_rgbt(0,0,0,0xFF)); }
			
			/// <summary>
			/// setup parameters of linear gradient of lines. <br/>
			/// GRAPHIN_RESULT SCFN(gLineGradientLinear)( HGFX hgfx, POS x1, POS y1, POS x2, POS y2, COLOR_STOP* stops, UINT nstops );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x1"></param>
			/// <param name="y1"></param>
			/// <param name="x2"></param>
			/// <param name="y2"></param>
			/// <param name="stops"></param>
			/// <param name="nstops"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gLineGradientLinear))]
			public delegate GRAPHIN_RESULT GraphicsLineGradientLinear(IntPtr hgfx, float x1, float y1, float x2, float y2, COLOR_STOP[] stops, uint nstops);

			// 
			/// <summary>
			/// setup parameters of linear gradient of fills. <br/>
			/// GRAPHIN_RESULT SCFN(gFillGradientLinear)( HGFX hgfx, POS x1, POS y1, POS x2, POS y2, COLOR_STOP* stops, UINT nstops );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x1"></param>
			/// <param name="y1"></param>
			/// <param name="x2"></param>
			/// <param name="y2"></param>
			/// <param name="stops"></param>
			/// <param name="nstops"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gFillGradientLinear))]
			public delegate GRAPHIN_RESULT GraphicsFillGradientLinear(IntPtr hgfx, float x1, float y1, float x2, float y2, COLOR_STOP[] stops, uint nstops);
			
			/// <summary>
			/// setup parameters of line gradient radial fills. <br/>
			/// GRAPHIN_RESULT SCFN(gLineGradientRadial)( HGFX hgfx, POS x, POS y, DIM rx, DIM ry, COLOR_STOP* stops, UINT nstops );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="rx"></param>
			/// <param name="ry"></param>
			/// <param name="stops"></param>
			/// <param name="nstops"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gLineGradientRadial))]
			public delegate GRAPHIN_RESULT GraphicsLineGradientRadial(IntPtr hgfx, float x, float y, float rx, float ry, COLOR_STOP[] stops, uint nstops);
			
			/// <summary>
			/// setup parameters of gradient radial fills. <br/>
			/// GRAPHIN_RESULT SCFN(gFillGradientRadial)( HGFX hgfx, POS x, POS y, DIM rx, DIM ry, COLOR_STOP* stops, UINT nstops );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="rx"></param>
			/// <param name="ry"></param>
			/// <param name="stops"></param>
			/// <param name="nstops"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gFillGradientRadial))]
			public delegate GRAPHIN_RESULT GraphicsFillGradientRadial(IntPtr hgfx, float x, float y, float rx, float ry, COLOR_STOP[] stops, uint nstops);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gFillMode) ( HGFX hgfx, BOOL even_odd );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="evenOdd">false - fill_non_zero</param>
			[SciterStructMap(nameof(SciterGraphicsApi.gFillMode))]
			public delegate GRAPHIN_RESULT GraphicsFillMode(IntPtr hgfx, bool evenOdd);

			#endregion

			#region SECTION: text
			
			/// <summary>
			/// create text layout using element's styles <br/>
			/// GRAPHIN_RESULT SCFN(textCreateForElement)(HTEXT* ptext, LPCWSTR text, UINT textLength, HELEMENT he );
			/// </summary>
			/// <param name="ptext"></param>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="he"></param>
			/// <param name="classNameOrNull"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.textCreateForElement))]
			public delegate GRAPHIN_RESULT TextCreateForElement(out IntPtr ptext, [MarshalAs(UnmanagedType.LPWStr)]string text, uint textLength, IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string classNameOrNull);

			/// <summary>
			/// create text layout using explicit format declaration <br/>
			/// GRAPHIN_RESULT SCFN(textCreateForElementAndStyle)(HTEXT* ptext, LPCWSTR text, UINT textLength, HELEMENT he, LPCWSTR style, UINT styleLength);
			/// </summary>
			/// <param name="ptext"></param>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="he"></param>
			/// <param name="style"></param>
			/// <param name="styleLength"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.textCreateForElementAndStyle))]
			public delegate GRAPHIN_RESULT TextCreateForElementAndStyle(out IntPtr ptext, [MarshalAs(UnmanagedType.LPWStr)]string text, uint textLength, IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string style, uint styleLength);

			[SciterStructMap(nameof(SciterGraphicsApi.textAddRef))]
			public delegate GRAPHIN_RESULT TextAddRef(IntPtr htext);
			
			[SciterStructMap(nameof(SciterGraphicsApi.textRelease))]
			public delegate GRAPHIN_RESULT TextRelease(IntPtr htext);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(textGetMetrics)(HTEXT text, DIM* minWidth, DIM* maxWidth, DIM* height, DIM* ascent, DIM* descent, UINT* nLines);
			/// </summary>
			/// <param name="htext"></param>
			/// <param name="minWidth"></param>
			/// <param name="maxWidth"></param>
			/// <param name="height"></param>
			/// <param name="ascent"></param>
			/// <param name="descent"></param>
			/// <param name="nLines"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.textGetMetrics))]
			public delegate GRAPHIN_RESULT TextGetMetrics(IntPtr htext, out float minWidth, out float maxWidth, out float height, out float ascent, out float descent, out uint nLines);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(textSetBox)(HTEXT text, DIM width, DIM height);
			/// </summary>
			/// <param name="htext"></param>
			/// <param name="width"></param>
			/// <param name="height"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.textSetBox))]
			public delegate GRAPHIN_RESULT TextSetBox(IntPtr htext, float width, float height);
			
			/// <summary>
			/// draw text with position (1..9 on MUMPAD) at px,py <br/>
			/// Ex: gDrawText( 100,100,5) will draw text box with its center at 100,100 px <br/>
			/// GRAPHIN_RESULT SCFN(gDrawText) ( HGFX hgfx, HTEXT text, POS px, POS py, UINT position );
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="text"></param>
			/// <param name="px"></param>
			/// <param name="py"></param>
			/// <param name="position"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gDrawText))]
			public delegate GRAPHIN_RESULT GraphicsDrawText(IntPtr hgfx, IntPtr text, float px, float py, uint position);

			#endregion

			#region SECTION: image rendering
			
			/// <summary>
			/// draws img onto the graphics surface with current transformation applied (scale, rotation). <br/>
			/// GRAPHIN_RESULT SCFN(gDrawImage) ( HGFX hgfx, HIMG himg, POS x, POS y, DIM* w, DIM* h, UINT* ix, UINT* iy, UINT* iw, UINT* ih, float* opacity );
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
			/// <param name="opacity">if provided is in 0.0 .. 1.0</param>
			[SciterStructMap(nameof(SciterGraphicsApi.gDrawImage))]
			public delegate GRAPHIN_RESULT GraphicsDrawImage(
				IntPtr hgfx,
				IntPtr himg,
				float x,
				float y,
				IntPtr w, //ref float w /*= 0*/,
				IntPtr h, //ref float h /*= 0*/,
				IntPtr ix, //ref uint ix /*= 0*/,
				IntPtr iy, //ref uint iy /*= 0*/,
				IntPtr iw, //ref uint iw /*= 0*/,
				IntPtr ih, //ref uint ih, /*= 0*/
				IntPtr opacity);// ref float opacity /*= 0, if provided is in 0.0 .. 1.0*/ );

			#endregion

			#region SECTION: coordinate space

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gWorldToScreen) ( HGFX hgfx, POS* inout_x, POS* inout_y);
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="inoutX"></param>
			/// <param name="inoutY"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gWorldToScreen))]
			public delegate GRAPHIN_RESULT GraphicsWorldToScreen(IntPtr hgfx, ref float inoutX, ref float inoutY);

			//inline GRAPHIN_RESULT
			//      graphics_world_to_screen ( HGFX hgfx, POS* length)
			//{
			//   return graphics_world_to_screen ( hgfx, length, 0);
			//}

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gScreenToWorld) ( HGFX hgfx, POS* inout_x, POS* inout_y);
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="inoutY"></param>
			/// <param name="inoutY"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gScreenToWorld))]
			public delegate GRAPHIN_RESULT GraphicsScreenToWorld(IntPtr hgfx, ref float inoutX, ref float inoutY);

			//inline GRAPHIN_RESULT
			//      graphics_screen_to_world ( HGFX hgfx, POS* length)
			//{
			//   return graphics_screen_to_world (hgfx, length, 0);
			//}

			#endregion

			#region SECTION: clipping

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gPushClipBox) ( HGFX hgfx, POS x1, POS y1, POS x2, POS y2, float opacity);
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="x1"></param>
			/// <param name="y1"></param>
			/// <param name="x2"></param>
			/// <param name="y2"></param>
			/// <param name="opacity">0.0 - 1.0</param>
			[SciterStructMap(nameof(SciterGraphicsApi.gPushClipBox))]
			public delegate GRAPHIN_RESULT GraphicsPushClipBox(IntPtr hgfx, float x1, float y1, float x2, float y2, float opacity = 1f);

			/// <summary>
			/// GRAPHIN_RESULT SCFN(gPushClipPath) ( HGFX hgfx, HPATH hpath, float opacity /*=1.f*/);
			/// </summary>
			/// <param name="hgfx"></param>
			/// <param name="hpath"></param>
			/// <param name="opacity"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gPushClipPath))]
			public delegate GRAPHIN_RESULT GraphicsPushClipPath(IntPtr hgfx, IntPtr hpath, float opacity = 1f);
			
			/// <summary>
			/// pop clip layer previously set by gPushClipBox or gPushClipPath <br/>
			/// GRAPHIN_RESULT SCFN(gPopClip) ( HGFX hgfx);
			/// </summary>
			/// <param name="hgfx"></param>
			[SciterStructMap(nameof(SciterGraphicsApi.gPopClip))]
			public delegate GRAPHIN_RESULT GraphicsPopClip(IntPtr hgfx);

			#endregion

			#region SECTION: image painter

			[SciterStructMap(nameof(SciterGraphicsApi.imagePaint))]
			public delegate GRAPHIN_RESULT ImagePaint(IntPtr himg, PublicDelegates.ImagePaintFunction pPainter, IntPtr prm);

			#endregion

			#region SECTION: VALUE interface

			[SciterStructMap(nameof(SciterGraphicsApi.vWrapGfx))]
			public delegate GRAPHIN_RESULT ValueWrapGfx(IntPtr hgfx, out SciterValue.VALUE toValue);

			[SciterStructMap(nameof(SciterGraphicsApi.vWrapImage))]
			public delegate GRAPHIN_RESULT ValueWrapImage(IntPtr himg, out SciterValue.VALUE toValue);

			[SciterStructMap(nameof(SciterGraphicsApi.vWrapPath))]
			public delegate GRAPHIN_RESULT ValueWrapPath(IntPtr hpath, out SciterValue.VALUE toValue);

			[SciterStructMap(nameof(SciterGraphicsApi.vWrapText))]
			public delegate GRAPHIN_RESULT ValueWrapText(IntPtr htext, out SciterValue.VALUE toValue);

			[SciterStructMap(nameof(SciterGraphicsApi.vUnWrapGfx))]
			public delegate GRAPHIN_RESULT ValueUnWrapGfx(ref SciterValue.VALUE fromValue, out IntPtr phgfx);

			[SciterStructMap(nameof(SciterGraphicsApi.vUnWrapImage))]
			public delegate GRAPHIN_RESULT ValueUnWrapImage(ref SciterValue.VALUE fromValue, out IntPtr phimg);

			[SciterStructMap(nameof(SciterGraphicsApi.vUnWrapPath))]
			public delegate GRAPHIN_RESULT ValueUnWrapPath(ref SciterValue.VALUE fromValue, out IntPtr phpath);

			[SciterStructMap(nameof(SciterGraphicsApi.vUnWrapText))]
			public delegate GRAPHIN_RESULT ValueUnWrapText(ref SciterValue.VALUE fromValue, out IntPtr phtext);

			#endregion
        }
    }
}