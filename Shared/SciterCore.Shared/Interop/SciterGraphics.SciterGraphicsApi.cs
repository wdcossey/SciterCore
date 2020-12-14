using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
    public static partial class SciterGraphics
    {
        [StructLayout(LayoutKind.Sequential)]
		internal readonly struct SciterGraphicsApi
		{
			public readonly SciterGraphicsApiDelegates.ImageCreate					imageCreate;
			public readonly SciterGraphicsApiDelegates.ImageCreateFromPixmap		imageCreateFromPixmap;
			public readonly SciterGraphicsApiDelegates.ImageAddRef					imageAddRef;
			public readonly SciterGraphicsApiDelegates.ImageRelease					imageRelease;
			public readonly SciterGraphicsApiDelegates.ImageGetInfo					imageGetInfo;
			public readonly SciterGraphicsApiDelegates.ImageClear					imageClear;
			public readonly SciterGraphicsApiDelegates.ImageLoad					imageLoad;
			public readonly SciterGraphicsApiDelegates.ImageSave					imageSave;
 	
			public readonly SciterGraphicsApiDelegates.Rgba							RGBA;
 	
			public readonly SciterGraphicsApiDelegates.GraphicsCreate				gCreate;
			public readonly SciterGraphicsApiDelegates.GraphicsAddRef				gAddRef;
			public readonly SciterGraphicsApiDelegates.GraphicsRelease				gRelease;
 	
			public readonly SciterGraphicsApiDelegates.GraphicsLine					gLine;
			public readonly SciterGraphicsApiDelegates.GraphicsRectangle			gRectangle;
			public readonly SciterGraphicsApiDelegates.GraphicsRoundedRectangle		gRoundedRectangle;
			public readonly SciterGraphicsApiDelegates.GraphicsEllipse				gEllipse;
			public readonly SciterGraphicsApiDelegates.GraphicsArc					gArc;
			public readonly SciterGraphicsApiDelegates.GraphicsStar					gStar;
			public readonly SciterGraphicsApiDelegates.GraphicsPolygon				gPolygon;
			public readonly SciterGraphicsApiDelegates.GraphicsPolyline				gPolyline;
 
			public readonly SciterGraphicsApiDelegates.PathCreate					pathCreate;
			public readonly SciterGraphicsApiDelegates.PathAddRef					pathAddRef;
			public readonly SciterGraphicsApiDelegates.PathRelease					pathRelease;
			public readonly SciterGraphicsApiDelegates.PathMoveTo					pathMoveTo;
			public readonly SciterGraphicsApiDelegates.PathLineTo					pathLineTo;
			public readonly SciterGraphicsApiDelegates.PathArcTo					pathArcTo;
			public readonly SciterGraphicsApiDelegates.PathQuadraticCurveTo			pathQuadraticCurveTo;
			public readonly SciterGraphicsApiDelegates.PathBezierCurveTo			pathBezierCurveTo;
			public readonly SciterGraphicsApiDelegates.PathClosePath				pathClosePath;
			public readonly SciterGraphicsApiDelegates.GraphicsDrawPath				gDrawPath;
 	
			public readonly SciterGraphicsApiDelegates.GraphicsRotate				gRotate;
			public readonly SciterGraphicsApiDelegates.GraphicsTranslate			gTranslate;
			public readonly SciterGraphicsApiDelegates.GraphicsScale				gScale;
			public readonly SciterGraphicsApiDelegates.GraphicsSkew					gSkew;
			public readonly SciterGraphicsApiDelegates.GraphicsTransform			gTransform;
 
			public readonly SciterGraphicsApiDelegates.GraphicsStateSave			gStateSave;
			public readonly SciterGraphicsApiDelegates.GraphicsStateRestore			gStateRestore;
 
			public readonly SciterGraphicsApiDelegates.GraphicsLineWidth			gLineWidth;
			public readonly SciterGraphicsApiDelegates.GraphicsLineJoin				gLineJoin;
			public readonly SciterGraphicsApiDelegates.GraphicsLineCap				gLineCap;
			public readonly SciterGraphicsApiDelegates.GraphicsLineColor			gLineColor;
			public readonly SciterGraphicsApiDelegates.GraphicsFillColor		  	gFillColor;
			public readonly SciterGraphicsApiDelegates.GraphicsLineGradientLinear 	gLineGradientLinear;
			public readonly SciterGraphicsApiDelegates.GraphicsFillGradientLinear	gFillGradientLinear;
			public readonly SciterGraphicsApiDelegates.GraphicsLineGradientRadial	gLineGradientRadial;
			public readonly SciterGraphicsApiDelegates.GraphicsFillGradientRadial	gFillGradientRadial;
			public readonly SciterGraphicsApiDelegates.GraphicsFillMode				gFillMode;
 
			public readonly SciterGraphicsApiDelegates.TextCreateForElement			textCreateForElement;
			public readonly SciterGraphicsApiDelegates.TextCreateForElementAndStyle textCreateForElementAndStyle;
			public readonly SciterGraphicsApiDelegates.TextAddRef					textAddRef;
			public readonly SciterGraphicsApiDelegates.TextRelease					textRelease;
			public readonly SciterGraphicsApiDelegates.TextGetMetrics				textGetMetrics;
			public readonly SciterGraphicsApiDelegates.TextSetBox					textSetBox;
			public readonly SciterGraphicsApiDelegates.GraphicsDrawText				gDrawText;
 
			public readonly SciterGraphicsApiDelegates.GraphicsDrawImage			gDrawImage; 

			public readonly SciterGraphicsApiDelegates.GraphicsWorldToScreen		gWorldToScreen;
			public readonly SciterGraphicsApiDelegates.GraphicsScreenToWorld		gScreenToWorld;
			
			public readonly SciterGraphicsApiDelegates.GraphicsPushClipBox			gPushClipBox;
			public readonly SciterGraphicsApiDelegates.GraphicsPushClipPath			gPushClipPath;
			public readonly SciterGraphicsApiDelegates.GraphicsPopClip				gPopClip;
			
			public readonly SciterGraphicsApiDelegates.ImagePaint					imagePaint;
			
			public readonly SciterGraphicsApiDelegates.ValueWrapGfx					vWrapGfx;
			public readonly SciterGraphicsApiDelegates.ValueWrapImage				vWrapImage;
			public readonly SciterGraphicsApiDelegates.ValueWrapPath				vWrapPath;
			public readonly SciterGraphicsApiDelegates.ValueWrapText				vWrapText;
			public readonly SciterGraphicsApiDelegates.ValueUnWrapGfx				vUnWrapGfx;
			public readonly SciterGraphicsApiDelegates.ValueUnWrapImage				vUnWrapImage;
			public readonly SciterGraphicsApiDelegates.ValueUnWrapPath				vUnWrapPath;
			public readonly SciterGraphicsApiDelegates.ValueUnWrapText				vUnWrapText;
		}
    }
}