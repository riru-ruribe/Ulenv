using System.Collections.Generic;

namespace Ulenv
{
    /// <summary>
    /// シーン間の受け渡しモデルです
    /// </summary>
    public interface ISceneGift
    {
        /// <summary>
        /// シーンのアドレスです
        /// </summary>
        string Address { get; }

        /// <summary>
        /// シーンの属するグループです
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// Addした場合にギフトが追加されていきます
        /// </summary>
        IList<ISceneGift> Childs { get; }
    }
}
