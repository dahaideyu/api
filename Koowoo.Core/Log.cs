using System;
using NLog;

namespace Koowoo.Core
{
    public static class Log
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();


        public static void Debug(object sender, string msg)
        {

            if (sender != null)
            {
                Type type = sender.GetType();
                logger.Debug(string.Format("{0}: {1}", type.FullName, msg));
            }
            else
            {
                logger.Debug(string.Format("{0}: {1}", "Null", msg));
            }

        }

        public static void Info(object sender, string msg)
        {
            if (sender != null)
            {
                Type type = sender.GetType();
                logger.Info(string.Format("{0}: {1}", type.FullName, msg));
            }
            else
            {
                logger.Info(string.Format("{0}: {1}", "Null", msg));
            }

        }

        //public static void Warn(object sender, string msg)
        //{
        //    if (sender != null)
        //    {
        //        Type type = sender.GetType();
        //        logger.Warn(string.Format("{0}: {1}", type.FullName, msg));
        //    }
        //    else
        //    {
        //        logger.Warn(string.Format("{0}: {1}", "Null", msg));
        //    }
        //}

        //public static void Error(object sender, Exception ex)
        //{
        //    if (sender != null)
        //    {
        //        Type type = sender.GetType();
        //        logger.Error(string.Format("{0}: {1}", type.FullName, ex));
        //    }
        //    else
        //    {
        //        logger.Error(string.Format("{0}: {1}", "Null", ex));
        //    }

        //}

        //public static void Fatal(object sender, string msg)
        //{
        //    if (sender != null)
        //    {
        //        Type type = sender.GetType();
        //        logger.Fatal(string.Format("{0}: {1}", type.FullName, msg));
        //    }
        //    else
        //    {

        //        logger.Fatal(string.Format("{0}: {1}", "Null", msg));
        //    }

        //}

        //public static void Trace(object sender, string msg)
        //{
        //    if (sender != null)
        //    {
        //        Type type = sender.GetType();
        //        logger.Trace(string.Format("{0}: {1}", type.FullName, msg));
        //    }
        //    else
        //    {
        //        logger.Trace(string.Format("{0}: {1}", "Null", msg));
        //    }

        //}


        public static void Debug(string msg)
        {
              logger.Debug(msg);
        }

        public static void Info(string msg)
        {
            logger.Info(msg);
        }

        public static void Warn(string msg)
        {
            logger.Warn(msg);
        }

        public static void Error(Exception ex,string message)
        {
            logger.Error(ex, message);
        }

        public static void Fatal(string msg)
        {
            logger.Fatal(msg);
        }

        public static void Trace(string msg)
        {
            logger.Trace(msg);
        }
    }
}