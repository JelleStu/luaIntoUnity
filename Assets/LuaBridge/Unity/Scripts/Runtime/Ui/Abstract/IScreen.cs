namespace Ui
{
    public interface IScreen
    {
        public IView AppView { get; }
        public IView PlayerView { get; }
    }
}