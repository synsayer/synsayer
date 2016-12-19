namespace FrameWork.View
{
    public abstract class ViewBase : FrameWork.Behaviour.CommonBehaviour
    {
        public ViewBase()
        {
            Controller.ControllerBase.init();
            Controller.ControllerBase.addView(this);  //controller�� View ���
        }
        virtual protected void Awake()
        {
            init();

        }

        virtual protected void OnDestroy()
        {
            if (enabled)
            {
                destroy();
                RemoveAllEventListener();
            }
        }

        public virtual void init()
        {
            registerEventListener();
        }

        public virtual void destroy()
        {
            unregisterEventListener();
            Controller.ControllerBase.removeView(this); //controller�� View �������
        }

        protected virtual void registerEventListener()
        {

        }

        protected virtual void unregisterEventListener()
        {

        }

    }
}