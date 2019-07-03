/* Author : Raphaël Marczak - 2016-2018
 * 
 * SPDX-License-Identifier: AGPL-3.0-or-later
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

