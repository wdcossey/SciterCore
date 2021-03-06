﻿using System;

#pragma warning disable 169
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedType.Local
// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
	class SciterXMsg
	{
		enum SCITER_X_MSG_CODE
		{
			SXM_CREATE = 0,
			SXM_DESTROY = 1,
			SXM_SIZE = 2,
			SXM_PAINT = 3,
		}

		struct SCITER_X_MSG
		{
			uint msg;
		}

		struct SCITER_X_MSG_CREATE
		{
			SCITER_X_MSG header;
			uint backend;
			bool transparent;
		}

		struct SCITER_X_MSG_DESTROY
		{
			SCITER_X_MSG header;
		}

		struct SCITER_X_MSG_SIZE
		{
			SCITER_X_MSG header;
			uint width;
			uint height;
		}

		enum SCITER_PAINT_TARGET_TYPE
		{
			SPT_DEFAULT = 0,
			SPT_RECEIVER = 1,
			SPT_DC = 2,
		}

		struct SCITER_X_MSG_PAINT
		{
			SCITER_X_MSG header;
			IntPtr element;// HELEMENT
			bool isFore;
			uint targetType;
			IntPtr hdc_or_param;// HDC or VOID*
			IntPtr callback;// ELEMENT_BITMAP_RECEIVER*
		}
	}
}