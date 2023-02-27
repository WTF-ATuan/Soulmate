using UnityEngine;
using Zenject;

namespace Game.Scripts{
	public class GameInstaller : MonoInstaller<GameInstaller>{
		public override void InstallBindings(){
			Container.Bind<CardRepository>().AsSingle().NonLazy();
		}
	}
}