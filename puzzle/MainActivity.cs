using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Views;
using System.Collections;
using System;

namespace puzzle
{
    [Activity(Label = "puzzle", MainLauncher = true,Icon ="@drawable/icon")]
    public class MainActivity : Activity
    {
        //region var

        Button resetButton;
        GridLayout mainLayout;

        int gameviewWidth;
        int tilewidth;
        ArrayList tilearr;
        ArrayList coordsarr;

        Point emptySpot;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            setGameView();
            makeTilesMethod();
            randomize();
        }

        private void setGameView()
        {
            resetButton = FindViewById<Button>(Resource.Id.resetbuttonid);
            resetButton.Click += ResetMethod;

            mainLayout = FindViewById<GridLayout>(Resource.Id.gamegridLayoutid);
            gameviewWidth = Resources.DisplayMetrics.WidthPixels;
            mainLayout.ColumnCount = 4;
            mainLayout.RowCount = 4;
            mainLayout.LayoutParameters = new LinearLayout.LayoutParams(gameviewWidth, gameviewWidth);
            mainLayout.SetBackgroundColor(Color.Gray);
        }

        private void makeTilesMethod()
        {
            tilewidth = gameviewWidth / 4;
            tilearr = new ArrayList();
            coordsarr = new ArrayList();

            int Counter = 1;
            // y as i
            //x as j


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    mytextView textTile = new mytextView(this);
                    GridLayout.Spec rowspec = GridLayout.InvokeSpec(j);
                    GridLayout.Spec colspec = GridLayout.InvokeSpec(i);
                    GridLayout.LayoutParams tilelayoutparams = new GridLayout.LayoutParams(rowspec, colspec);

                    textTile.Text = Counter.ToString();
                    textTile.SetTextColor(Color.Black);
                    textTile.TextSize = 40;
                    textTile.Gravity = GravityFlags.Center;
                    tilelayoutparams.Width = tilewidth - 10;
                    tilelayoutparams.Height = tilewidth - 10;
                    tilelayoutparams.SetMargins(5, 5, 5, 5);

                    textTile.LayoutParameters = tilelayoutparams;

                    textTile.SetBackgroundColor(Color.Orange);

                    Point thislocation = new Point(i, j);
                    coordsarr.Add(thislocation);

                    textTile.xPos = thislocation.X;
                    textTile.yPos = thislocation.Y;


                    textTile.Touch += TextTile_Touch;

                    tilearr .Add(textTile);

                    mainLayout.AddView(textTile);
                    Counter++;

                }
            }


            mainLayout.RemoveView((mytextView)tilearr[15]);
            tilearr.RemoveAt(15);
            

        }

        void TextTile_Touch(object sender, View.TouchEventArgs e)
        {
           if(e.Event.Action==MotionEventActions.Up)
            {

                mytextView thistile = (mytextView)sender;
                float xdif = (float)Math.Pow(thistile.xPos - emptySpot.X, 2);
                float ydif = (float)Math.Pow(thistile.yPos - emptySpot.Y, 2);
                float dist = (float)Math.Sqrt(xdif + ydif);

                if (dist == 1)
                {
                    //tile can move
                    Point curpoint = new Point(thistile.xPos, thistile.yPos);
                    GridLayout.Spec rowSpec = GridLayout.InvokeSpec(emptySpot.Y);
                    GridLayout.Spec colspec = GridLayout.InvokeSpec(emptySpot.X);

                    GridLayout.LayoutParams newlocalparams = new GridLayout.LayoutParams(rowSpec, colspec);

                    newlocalparams.Width = tilewidth - 10;
                    newlocalparams.Height = tilewidth - 10;
                    newlocalparams.SetMargins(5, 5, 5, 5);

                    thistile .xPos = emptySpot .X;
                   thistile.yPos = emptySpot.Y;

                    thistile .LayoutParameters = newlocalparams;

                    emptySpot = curpoint;

                }
                else
                {
                    //nothing happened

                }

            }
        }

        private void randomize()
        {
            ArrayList tempcoordsarr = new ArrayList(coordsarr);

            Random myrand= new Random();
            foreach(mytextView any in tilearr)
            {
                int randindex = myrand.Next(0, tempcoordsarr.Count);
                Point thisrandomloc = (Point)tempcoordsarr[randindex];
                GridLayout.Spec rowspec = GridLayout.InvokeSpec(thisrandomloc.Y);
                GridLayout.Spec colspec = GridLayout.InvokeSpec(thisrandomloc.X);


                GridLayout.LayoutParams randLayoutparams = new GridLayout.LayoutParams(rowspec, colspec);

                randLayoutparams.Width = tilewidth - 10;
                randLayoutparams.Height = tilewidth - 10;
                randLayoutparams.SetMargins(5, 5, 5, 5);

                any.xPos = thisrandomloc.X;
                any.yPos = thisrandomloc.Y;

                any.LayoutParameters = randLayoutparams;

                tempcoordsarr.RemoveAt(randindex);
            }
            emptySpot = (Point)tempcoordsarr[0];
        }

        void ResetMethod(object sender, System.EventArgs e)
        {
            randomize();
        }
    }

class mytextView : TextView
    {
        Activity myContext;
    public mytextView(Activity context):base(context)
        {
            myContext = context;

        }
        public int xPos { set; get; }
        public int yPos { set; get; }

    }



}

