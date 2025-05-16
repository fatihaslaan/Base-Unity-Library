namespace AG.Base.Util
{
    public interface IPoolObject
    {
        public void OnObjectReturn();
        public void OnObjectGet();
    }
}