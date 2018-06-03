﻿using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

namespace PinupMobile.Droid.Controls
{
    [Register("PinupMobile.Droid.AutoFitTextureView")]
    public class AutoFitTextureView : TextureView
    {
        private int _ratioWidth = 0;
        private int _ratioHeight = 0;

        public AutoFitTextureView(Context context)
            : this(context, null)
        {
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0)
        {
        }

        public AutoFitTextureView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        public void SetAspectRatio(int width, int height)
        {
            if (width == 0 || height == 0)
            {
                throw new ArgumentException("Size cannot be negative.");
            }

            _ratioWidth = width;
            _ratioHeight = height;
            RequestLayout();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = MeasureSpec.GetSize(heightMeasureSpec);
            if (_ratioWidth == 0 || _ratioHeight == 0)
            {
                SetMeasuredDimension(width, height);
            }
            else
            {
                if (width < (float)height * _ratioWidth / (float)_ratioHeight)
                {
                    SetMeasuredDimension(width, width * _ratioHeight / _ratioWidth);
                }
                else
                {
                    SetMeasuredDimension(height * _ratioWidth / _ratioHeight, height);
                }
            }
        }
    }
}
