namespace MusicBox.Services
{
    public class MusicBoxVisitCounter
    
       
        {
            private static readonly Lazy<MusicBoxVisitCounter>
                _instance = new(() => new MusicBoxVisitCounter());

            public static MusicBoxVisitCounter Instance =>
                _instance.Value;

            private int _visits;

            // Singleton Design Pattern:
            // private constructor prevents external instantiation.
            // Lazy<T> creates one shared instance safely.
            private MusicBoxVisitCounter()
            {
            }

            public int RegisterVisit()
            {
                return System.Threading.Interlocked.Increment(
                    ref _visits);
            }
        }
    }


