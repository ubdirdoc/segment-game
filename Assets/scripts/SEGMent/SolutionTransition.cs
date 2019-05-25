/* Author : Raphaël Marczak - 2016-2018
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/4.0/ 
 * or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA. 
 * 
 */

using System.Collections;
using System.Collections.Generic;

namespace SEGMent {
	public class SolutionTransition : GraphTransition {
		private string m_question;
		private List<string> m_solutions;
		private Dictionary<string, string> m_wrongAnswers;
		private bool m_isPassword;

		
		public SolutionTransition(GraphNode from, GraphNode to, string question, List<string> solutions, Dictionary<string, string> wrongAnswers, bool isPassword, bool isImmediate, bool isUnique):base (from, to, isImmediate, isUnique)
		{
			m_question = question;
			m_solutions = solutions;
			m_wrongAnswers = wrongAnswers;
			m_isPassword = isPassword;

			SetTransitionType(TRANSITION_TYPE.SOLUTION_TYPE);
		}

		public List<string> GetSolutions() {
			return m_solutions;
		}

		public string GetQuestion() {
			return m_question;
		}

		public Dictionary<string, string> GetWrongAnswerToHelp() {
			return m_wrongAnswers;
		}

		public bool IsPassword() {
			return m_isPassword;
		}
		
	}
}