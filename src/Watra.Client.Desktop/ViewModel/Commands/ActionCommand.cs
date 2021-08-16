// Copyright (c) ZHAW, Marco Bertschi, Patrick Stadler. All rights reserved.

namespace Watra.Client.Desktop.ViewModel.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// A general <see cref="ICommand"/> implementation
    /// which executes an action.
    /// </summary>
    public class ActionCommand : ICommand
    {
        private readonly Action action;
        private readonly Func<bool> canExecuteFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommand"/> class.
        /// </summary>
        public ActionCommand(Action action, Func<bool> canExecuteFunc)
        {
            this.action = action;
            this.canExecuteFunc = canExecuteFunc;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return this.canExecuteFunc();
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            this.action.Invoke();
        }

        /// <summary>
        /// Prompts any controls bound to this command to reevaluate
        /// if the command can execute and refresh themselves if neccessary.
        /// </summary>
        public void ReevaluateCanExecute()
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
