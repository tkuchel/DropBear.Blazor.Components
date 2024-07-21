#region

using System.Globalization;
using DropBear.Blazor.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

namespace DropBear.Blazor.Components.Components.Modals;

public partial class StandardModal : ComponentBase, IAsyncDisposable
{
    private ElementReference _modalElement;

    [Parameter] public string Title { get; set; } = "Modal Title";
    [Parameter] public RenderFragment? BodyContent { get; set; }
    [Parameter] public RenderFragment? CustomHeader { get; set; }
    [Parameter] public RenderFragment? CustomFooter { get; set; }
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    [Parameter] public string CancelText { get; set; } = "Cancel";
    [Parameter] public bool ShowCancelButton { get; set; } = true;
    [Parameter] public bool CloseOnBackdropClick { get; set; } = true;
    [Parameter] public EventCallback OnConfirm { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public bool IsLightMode { get; set; }
    [Parameter] public ModalSize Size { get; set; } = ModalSize.Medium;
    [Parameter] public string CustomCssClass { get; set; } = "";
    [Parameter] public ModalTransition Transition { get; set; } = ModalTransition.Fade;
    [Parameter] public bool ScrollableContent { get; set; }

    private bool IsVisible { get; set; }

    #region IAsyncDisposable Members

    public ValueTask DisposeAsync()
    {
        // Cleanup code if needed
        return ValueTask.CompletedTask;
    }

    #endregion

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await InitializeJavaScript();
        }
    }

    private async Task InitializeJavaScript()
    {
        await JsRuntime.InvokeVoidAsync("eval", @"
            window.standardModal = {
                initialize: function(element) {
                    if (element && element.classList) {
                        element.querySelector('.modal-close')?.addEventListener('click', () => this.hide(element));
                        element.addEventListener('click', event => {
                            if (event.target === element) {
                                this.hide(element);
                            }
                        });
                    } else {
                        console.error('Invalid element passed to StandardModal.initialize');
                    }
                },
                show: function(element) {
                    if (element && element.classList) {
                        element.style.display = 'flex';
                        setTimeout(() => {
                            element.classList.add('active');
                            this.trapFocus(element);
                        }, 10);
                    }
                },
                hide: function(element) {
                    if (element && element.classList) {
                        element.classList.remove('active');
                        setTimeout(() => {
                            element.style.display = 'none';
                        }, 300);
                    }
                },
                trapFocus: function(element) {
                    const focusableElements = element.querySelectorAll('button, [href], input, select, textarea, [tabindex]:not([tabindex=""-1""])');
                    const firstFocusableElement = focusableElements[0];
                    const lastFocusableElement = focusableElements[focusableElements.length - 1];

                    element.addEventListener('keydown', e => {
                        if (e.key === 'Tab') {
                            if (e.shiftKey) {
                                if (document.activeElement === firstFocusableElement) {
                                    lastFocusableElement.focus();
                                    e.preventDefault();
                                }
                            } else {
                                if (document.activeElement === lastFocusableElement) {
                                    firstFocusableElement.focus();
                                    e.preventDefault();
                                }
                            }
                        }

                        if (e.key === 'Escape') {
                            this.hide(element);
                        }
                    });

                    firstFocusableElement.focus();
                }
            };
        ");
        await JsRuntime.InvokeVoidAsync("standardModal.initialize", _modalElement);
    }


    public async Task ShowAsync()
    {
        IsVisible = true;
        StateHasChanged();
        await Task.Delay(10); // Allow time for the DOM to update
        await JsRuntime.InvokeVoidAsync("standardModal.show", _modalElement);
    }

    public void Show()
    {
        _ = ShowAsync();
    }

    public async Task HideAsync()
    {
        await JsRuntime.InvokeVoidAsync("standardModal.hide", _modalElement);
        IsVisible = false;
        StateHasChanged();
    }

    public void Hide()
    {
        _ = HideAsync();
    }

    private async Task CloseClick()
    {
        if (CloseOnBackdropClick)
        {
            await OnClose.InvokeAsync();
            await HideAsync();
        }
    }

    private async Task ConfirmClick()
    {
        await OnConfirm.InvokeAsync();
        await HideAsync();
    }

    private async Task CancelClick()
    {
        await OnCancel.InvokeAsync();
        await HideAsync();
    }

    private string GetModalClasses()
    {
        var classes = new List<string>
        {
            "standard-modal",
            IsVisible ? "active" : "",
            IsLightMode ? "theme-light" : "theme-dark",
            CustomCssClass,
            Size.ToString().ToLower(CultureInfo.CurrentCulture),
            Transition.ToString().ToLower(CultureInfo.CurrentCulture),
            ScrollableContent ? "scrollable" : ""
        };

        return string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
    }
}
