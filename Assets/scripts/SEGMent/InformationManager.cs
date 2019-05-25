/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent
{
	public enum PLAYER_MODIF_INFO {PLAYER_MOVED};

	public class InformationManager
	{
		private List<PLAYER_MODIF_INFO> m_playerModifInfos;
		private List<string> m_transitionSoundsToPlay;

		public InformationManager ()
		{
			m_playerModifInfos = new List<PLAYER_MODIF_INFO>();
			m_transitionSoundsToPlay = new List<string>();
		}


		public void AddPlayerModifInfo(PLAYER_MODIF_INFO info) {
			m_playerModifInfos.Add(info);
		}

		public List<PLAYER_MODIF_INFO> PopPlayerModifInfos() {
			List<PLAYER_MODIF_INFO> infosToReturn = new List<PLAYER_MODIF_INFO>(m_playerModifInfos);

			m_playerModifInfos.Clear();

			return infosToReturn;
		}

		public void AddTransitionSoundsToPlay(string soundName) {
			m_transitionSoundsToPlay.Add(soundName);
		}

		public List<string> PopTransitionSoundsToPlay() {
			List<string> soundsInfoReturn = new List<string>(m_transitionSoundsToPlay);

			m_transitionSoundsToPlay.Clear();

			return soundsInfoReturn;
		}
	}
}

