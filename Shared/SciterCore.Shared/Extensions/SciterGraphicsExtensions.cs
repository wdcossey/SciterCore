using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    public static class SciterGraphicsExtensions
    {

        #region Blend

        public static SciterGraphics BlendImage(this SciterGraphics graphics, SciterImage img, float x = 0f, float y = 0f)
        {
            graphics?.BlendImageInternal(img: img, x: x, y: y);
            return graphics;
        }

        public static bool TryBlendImage(this SciterGraphics graphics, SciterImage img, float x = 0f, float y = 0f)
        {
            return graphics?.TryBlendImageInternal(img: img, x: x, y: y) == true;
        }

        #endregion

        #region Convert

        public static SciterValue ToValue(this SciterGraphics graphics)
        {
            return graphics?.ToValueInternal();
        }

        public static bool TryToValue(this SciterGraphics graphics, out SciterValue value)
        {
            value = default;
            return graphics?.TryToValueInternal(sciterValue: out value) == true;
        }
        
        #endregion Convert
        
        #region Draw Geometries

        public static SciterGraphics DrawRectangle(this SciterGraphics graphics, float x1, float y1, float x2, float y2)
        {
            graphics?.DrawRectangleInternal(x1: x1, y1: y1, x2: x2, y2: y2);
            return graphics;
        }

        public static bool TryDrawRectangle(this SciterGraphics graphics, float x1, float y1, float x2, float y2)
        {
            return graphics?.TryDrawRectangleInternal(x1: x1, y1: y1, x2: x2, y2: y2) == true;
        }

        public static SciterGraphics DrawLine(this SciterGraphics graphics, float x1, float y1, float x2, float y2)
        {
            graphics?.DrawLineInternal(x1: x1, y1: y1, x2: x2, y2: y2);
            return graphics;
        }

        public static bool TryDrawLine(this SciterGraphics graphics, float x1, float y1, float x2, float y2)
        {
            return graphics?.TryDrawLineInternal(x1: x1, y1: y1, x2: x2, y2: y2) == true;
        }

        public static SciterGraphics DrawPolygon(this SciterGraphics graphics, IEnumerable<PolygonPoint> points)
        {
            if (points?.Any() != true)
                throw new ArgumentNullException(nameof(points), @"Cannot be null.");
            
            graphics?.DrawPolygonInternal(points: points);
            return graphics;
        }
        
        public static SciterGraphics DrawPolygon(this SciterGraphics graphics, params PolygonPoint[] points)
        {
            return graphics?.DrawPolygon(points: points?.AsEnumerable());
        }

        public static SciterGraphics DrawPolygon(this SciterGraphics graphics, Func<SciterGraphics, IEnumerable<PolygonPoint>> pointsFunc)
        {
            if (pointsFunc == null)
                throw new ArgumentNullException(nameof(pointsFunc), @"Cannot be null.");
            
            return graphics?.DrawPolygon(points: pointsFunc.Invoke(graphics)?.ToArray());
        }

        public static SciterGraphics DrawPolygon(this SciterGraphics graphics, Func<IEnumerable<PolygonPoint>> pointsFunc)
        {
            if (pointsFunc == null)
                throw new ArgumentNullException(nameof(pointsFunc), @"Cannot be null.");
            
            return graphics?.DrawPolygon(points: pointsFunc.Invoke()?.ToArray());
        }

        public static bool TryDrawPolygon(this SciterGraphics graphics, IEnumerable<PolygonPoint> points)
        {
            return graphics?.TryDrawPolygonInternal(points: points) == true;
        }

        public static bool TryDrawPolygon(this SciterGraphics graphics, Func<IEnumerable<PolygonPoint>> pointsFunc)
        {
            if (pointsFunc == null)
                throw new ArgumentNullException(nameof(pointsFunc), @"Cannot be null.");
            
            return graphics?.TryDrawPolygon(points: pointsFunc.Invoke()) == true;
        }

        
        public static SciterGraphics DrawPolyline(this SciterGraphics graphics, IEnumerable<PolylinePoint> points)
        {
            if (points?.Any() != true)
                throw new ArgumentNullException(nameof(points), @"Cannot be null.");
            
            graphics?.DrawPolylineInternal(points: points);
            return graphics;
        }
        
        public static SciterGraphics DrawPolyline(this SciterGraphics graphics, PolylinePoint[] points)
        {
            return graphics?.DrawPolyline(points: points?.AsEnumerable());
        }

        public static SciterGraphics DrawPolyline(this SciterGraphics graphics, Func<IEnumerable<PolylinePoint>> pointsFunc)
        {
            if (pointsFunc == null)
                throw new ArgumentNullException(nameof(pointsFunc), @"Cannot be null.");
            
            graphics?.DrawPolyline(points: pointsFunc.Invoke());
            return graphics;
        }

        public static bool TryDrawPolyline(this SciterGraphics graphics, IEnumerable<PolylinePoint> points)
        {
            return graphics?.TryDrawPolylineInternal(points: points) == true;
        }

        public static bool TryDrawPolyline(this SciterGraphics graphics, Func<IEnumerable<PolylinePoint>> pointsFunc)
        {
            if (pointsFunc == null)
                throw new ArgumentNullException(nameof(pointsFunc), @"Cannot be null.");
            
            return graphics?.TryDrawPolyline(points: pointsFunc.Invoke()) == true;
        }

        public static SciterGraphics DrawEllipse(this SciterGraphics graphics, float x, float y, float rx, float ry)
        {
            graphics?.DrawEllipseInternal(x: x, y: y, rx: rx, ry: ry);
            return graphics;
        }

        public static bool TryDrawEllipse(this SciterGraphics graphics, float x, float y, float rx, float ry)
        {
            return graphics?.TryDrawEllipseInternal(x: x, y: y, rx: rx, ry: ry) == true;
        }

        #endregion Draw Geometries

        #region Drawing Attributes

        public static SciterGraphics SetLineWidth(this SciterGraphics graphics, float lineWidth)
        {
            graphics?.SetLineWidthInternal(lineWidth: lineWidth);
            return graphics;
        }

        public static bool TrySetLineWidth(this SciterGraphics graphics, float lineWidth)
        {
            return graphics?.TrySetLineWidthInternal(lineWidth: lineWidth) == true;
        }

        public static SciterGraphics SetLineJoin(this SciterGraphics graphics, LineJoinType joinType)
        {
            graphics?.SetLineJoinInternal(joinType: joinType);
            return graphics;
        }

        public static bool TrySetLineJoin(this SciterGraphics graphics, LineJoinType joinType)
        {
            return graphics?.TrySetLineJoinInternal(joinType: joinType) == true;
        }

        public static SciterGraphics SetLineCap(this SciterGraphics graphics, LineCapType capType)
        {
            graphics?.SetLineCapInternal(capType: capType);
            return graphics;
        }

        public static bool TrySetLineCap(this SciterGraphics graphics, LineCapType capType)
        {
            return graphics?.TrySetLineCapInternal(capType: capType) == true;
        }

        public static SciterGraphics SetLineColor(this SciterGraphics graphics, SciterColor lineColor)
        {
            graphics?.SetLineColorInternal(lineColor: lineColor);
            return graphics;
        }
        
        public static SciterGraphics SetLineColor(this SciterGraphics graphics, byte r, byte g, byte b)
        {
            graphics?.SetLineColor(r: r, g: g, b: b, alpha: 1f);
            return graphics;
        }
        
        public static SciterGraphics SetLineColor(this SciterGraphics graphics, byte r, byte g, byte b, float alpha)
        {
            graphics?.SetLineColor(lineColor: new SciterColor(r: r, g: g, b: b, alpha: alpha));
            return graphics;
        }

        public static SciterGraphics SetLineColor(this SciterGraphics graphics, byte r, byte g, byte b, byte alpha)
        {
            graphics?.SetLineColor(lineColor: new SciterColor(r: r, g: g, b: b, alpha: alpha));
            return graphics;
        }

        public static bool TrySetLineColor(this SciterGraphics graphics, SciterColor lineColor)
        {
            return graphics?.TrySetLineColorInternal(lineColor: lineColor) == true;
        }

        public static bool TrySetLineColor(this SciterGraphics graphics, byte r, byte g, byte b)
        {
            return graphics?.TrySetLineColor(r: r, g: g, b: b, alpha: 1f) == true;
        }

        public static bool TrySetLineColor(this SciterGraphics graphics, byte r, byte g, byte b, float alpha = 1f)
        {
            return graphics?.TrySetLineColor(lineColor: new SciterColor(r: r, g: g, b: b, alpha: alpha)) == true;
        }

        public static bool TrySetLineColor(this SciterGraphics graphics, byte r, byte g, byte b, byte alpha = 255)
        {
            return graphics?.TrySetLineColor(lineColor: new SciterColor(r: r, g: g, b: b, alpha: alpha)) == true;
        }
        
        public static SciterGraphics SetLineGradientLinear(this SciterGraphics graphics, float x1, float y1, float x2, float y2,
            params SciterColorStop[] stops)
        {
            graphics?.SetLineGradientLinearInternal(x1: x1, y1: y1, x2: x2, y2: y2, stops: stops);
            return graphics;
        }
        
        public static bool TrySetLineGradientLinear(this SciterGraphics graphics, float x1, float y1, float x2, float y2,
            params SciterColorStop[] stops)
        {
            return graphics?.TrySetLineGradientLinearInternal(x1: x1, y1: y1, x2: x2, y2: y2, stops: stops) == true;
        }
        
        public static SciterGraphics SetFillGradientLinear(this SciterGraphics graphics, float x1, float y1, float x2, float y2,
            params SciterColorStop[] stops)
        {
            graphics?.SetFillGradientLinearInternal(x1: x1, y1: y1, x2: x2, y2: y2, stops: stops);
            return graphics;
        }
        
        public static bool TrySetFillGradientLinear(this SciterGraphics graphics, float x1, float y1, float x2, float y2,
            params SciterColorStop[] stops)
        {
            return graphics?.TrySetFillGradientLinearInternal(x1: x1, y1: y1, x2: x2, y2: y2, stops: stops) == true;
        }

        public static SciterGraphics SetFillColor(this SciterGraphics graphics, SciterColor fillColor)
        {
            graphics?.SetFillColorInternal(fillColor: fillColor);
            return graphics;
        }

        public static SciterGraphics SetFillColor(this SciterGraphics graphics, byte r, byte g, byte b)
        {
            graphics?.SetFillColor(fillColor: new SciterColor(r: r, g: g, b: b, alpha: 1f));
            return graphics;
        }

        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static SciterGraphics SetFillColor(this SciterGraphics graphics, byte r, byte g, byte b, float alpha = 1f)
        {
            graphics?.SetFillColor(fillColor: new SciterColor(r: r, g: g, b: b, alpha: alpha));
            return graphics;
        }

        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static SciterGraphics SetFillColor(this SciterGraphics graphics, byte r, byte g, byte b, byte alpha = byte.MaxValue)
        {
            graphics?.SetFillColor(fillColor: new SciterColor(r: r, g: g, b: b, alpha: alpha));
            return graphics;
        }

        public static bool TrySetFillColor(this SciterGraphics graphics, SciterColor fillColor)
        {
            return graphics?.TrySetFillColorInternal(fillColor: fillColor) == true;
        }
        
        public static bool TrySetFillColor(this SciterGraphics graphics, byte r, byte g, byte b, float alpha = 1f)
        {
            return graphics?.TrySetFillColor(fillColor: new SciterColor(r: r, g: g, b: b, alpha: alpha)) == true;
        }

        public static bool TrySetFillColor(this SciterGraphics graphics, byte r, byte g, byte b, byte alpha = 255)
        {
            return graphics?.TrySetFillColor(fillColor: new SciterColor(r: r, g: g, b: b, alpha: alpha)) == true;
        }
        
        #endregion Drawing Attributes

        #region Path Operations

        public static SciterGraphics DrawPath(this SciterGraphics graphics, SciterPath path, DrawPathMode pathMode)
        {
            graphics?.DrawPathInternal(path: path, pathMode: pathMode);
            return graphics;
        }

        public static bool TryDrawPath(this SciterGraphics graphics, SciterPath path, DrawPathMode pathMode)
        {
            return graphics?.TryDrawPathInternal(path: path, pathMode: pathMode) == true;
        }

        #endregion
        
        #region Affine tranformations
        
        public static SciterGraphics Rotate(this SciterGraphics graphics, float radians, float cx = 0f, float cy = 0f)
        {
            graphics?.RotateInternal(radians: radians, cx: cx, cy: cy);
            return graphics;
        }
		
        public static bool TryRotate(this SciterGraphics graphics, float radians, float cx = 0f, float cy = 0f)
        {
            return graphics?.TryRotateInternal(radians: radians, cx: cx, cy: cy) == true; 
        }

        public static SciterGraphics Translate(this SciterGraphics graphics, float cx, float cy)
        {
            graphics?.TranslateInternal(cx: cx, cy: cy);
            return graphics;
        }

        public static bool TryTranslate(this SciterGraphics graphics, float cx, float cy)
        {
            return graphics?.TryTranslateInternal(cx: cx, cy: cy) == true; 
        }

        public static SciterGraphics Scale(this SciterGraphics graphics, float x, float y)
        {
            graphics?.ScaleInternal(x: x, y: y);
            return graphics;
        }

        public static bool TryScale(this SciterGraphics graphics, float x, float y)
        {
            return graphics?.TryScaleInternal(x: x, y: y) == true; 
        }

        public static SciterGraphics SkewInternal(this SciterGraphics graphics, float dx, float dy)
        {
            graphics?.SkewInternal(dx: dx, dy: dy);
            return graphics;
        }

        public static bool TrySkewInternal(this SciterGraphics graphics, float dx, float dy)
        {
            return graphics?.TrySkewInternal(dx: dx, dy: dy) == true;
        }
        
        #endregion Affine tranformations
        
        #region Text
        
        public static SciterGraphics DrawText(this SciterGraphics graphics, SciterText text, float px, float py, uint position)
        {
            graphics?.DrawTextInternal(text: text, px: px, py: py, position: position);
            return graphics;
        }
		
        public static bool TryDrawText(this SciterGraphics graphics, SciterText text, float px, float py, uint position)
        {
            return graphics?.TryDrawTextInternal(text: text, px: px, py: py, position: position) == true;
        }
        
        #endregion Text
        
        #region Clipping
        
        public static SciterGraphics PushClipBox(this SciterGraphics graphics, float x1, float y1, float x2, float y2, float opacity = 1)
        {
            graphics?.PushClipBoxInternal(x1: x1, y1: y1, x2: x2, y2: y2, opacity: opacity);
            return graphics;
        }
		
        public static bool TryPushClipBox(this SciterGraphics graphics, float x1, float y1, float x2, float y2, float opacity = 1)
        {
            return graphics?.TryPushClipBoxInternal(x1: x1, y1: y1, x2: x2, y2: y2, opacity: opacity) == true;
        }

        public static SciterGraphics PushClipPath(this SciterGraphics graphics, SciterPath path, float opacity = 1)
        {
            graphics?.PushClipPathInternal(path: path, opacity: opacity);
            return graphics;
        }

        public static bool TryPushClipPath(this SciterGraphics graphics, SciterPath path, float opacity = 1)
        {
            return graphics?.TryPushClipPathInternal(path: path, opacity: opacity) == true;
        }

        public static SciterGraphics PopClipInternal(this SciterGraphics graphics)
        {
            graphics?.PopClipInternal();
            return graphics;
        }

        public static bool TryPopClip(this SciterGraphics graphics)
        {
            return graphics?.TryPopClipInternal() == true;
        }

        #endregion Clipping
        
        #region State
        
        public static SciterGraphics SaveState(this SciterGraphics graphics)
        {
            graphics?.SaveStateInternal();
            return graphics;
        }
		
        public static bool TrySaveState(this SciterGraphics graphics)
        {
            return graphics?.TrySaveStateInternal() == true;
        }

        public static SciterGraphics RestoreState(this SciterGraphics graphics)
        {
            graphics?.RestoreStateInternal();
            return graphics;
        }

        public static bool TryRestoreState(this SciterGraphics graphics)
        {
            return graphics?.TryRestoreStateInternal() == true;
        }

        public static SciterGraphics Using(this SciterGraphics graphics, Action<SciterGraphics> action)
        {
            action?.Invoke(graphics);
            return graphics;
        }
        
        #endregion State

    }
}