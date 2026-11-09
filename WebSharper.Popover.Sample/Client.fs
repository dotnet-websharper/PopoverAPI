namespace WebSharper.Popover.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.Popover

[<JavaScript>]
module Client =
    // The template is loaded from the DOM, so editing index.html and refreshing
    // the browser is enough; a rebuild is only needed when holes change.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let popover () =
        JS.Document.GetElementById("myPopover") |> As<HTMLElement>

    [<SPAEntryPoint>]
    let Main () =
        IndexTemplate()
            .ShowBtn(fun _ -> popover().ShowPopover())
            .HideBtn(fun _ -> popover().HidePopover())
            .Bind()
