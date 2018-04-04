
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Java.Util;
using Android.Renderscripts;
using Android.Support.V7.App;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace NFS
{
	[Activity(Label = "LoginAndRegisterActivity", Theme = "@style/MyTheme",MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class LoginAndRegisterActivity : AppCompatActivity
	{
		ImageView imgbgL;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.LoginAndRegisterLayout);

			AppCenter.Start("98cc9f60-5c17-498e-b3d0-ea5c41b190b0",typeof(Analytics), typeof(Crashes));

			FnInitialization();
			FnBlurBackgroundImage();
		}

		void FnInitialization()
		{
			imgbgL = FindViewById<ImageView>(Resource.Id.imgBgLL);
		}

		void FnBlurBackgroundImage()
		{ 
			Bitmap bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.wallpaper10);
			Bitmap blurredBitmap = FnBlurEffectToImage(bitmap);
			imgbgL.SetImageBitmap(blurredBitmap);
		}

		public Bitmap FnBlurEffectToImage(Bitmap image)
		{
			if (image == null) return null;

			Bitmap outputBitmap = Bitmap.CreateBitmap(image);
			RenderScript renderScript = RenderScript.Create(this);
			Allocation tmpIn = Allocation.CreateFromBitmap(renderScript, image);
			Allocation tmpOut = Allocation.CreateFromBitmap(renderScript, outputBitmap);
			//Intrinsic Gausian blur filter
			ScriptIntrinsicBlur theIntrinsic = ScriptIntrinsicBlur.Create(renderScript, Element.U8_4(renderScript));
			theIntrinsic.SetRadius(10f);
			theIntrinsic.SetInput(tmpIn);
			theIntrinsic.ForEach(tmpOut);
			tmpOut.CopyTo(outputBitmap);
			return outputBitmap;

		}
	}
}
