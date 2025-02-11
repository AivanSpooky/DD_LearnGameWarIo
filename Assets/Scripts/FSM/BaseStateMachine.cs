using System.Collections.Generic;
using Assets.Scripts.Exceptions;

namespace LearnGame.FSM
{
    public class BaseStateMachine
    {
        private BaseState _curState;
        private List<BaseState> _states;
        private Dictionary<BaseState, List<Transition>> _transitions;

        public BaseStateMachine()
        {
            _states = new List<BaseState>();
            _transitions = new Dictionary<BaseState, List<Transition>>();
        }

        public void SetInitialState(BaseState state) => _curState = state;
        public void AddState(BaseState state, List<Transition> transitions)
        {
            if (!_states.Contains(state))
            {
                _states.Add(state);
                _transitions.Add(state, transitions);
            }
            else
                throw new AlreadyExistsException($"State {state.GetType()} already exists in state machine!");
        }

        public void Update()
        {
            foreach (var transition in _transitions[_curState])
                if (transition.Condition())
                {
                    _curState = transition.ToState;
                    break;
                }
            _curState.Execute();
        }
    }
}
