namespace WebSharper.Popover

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    module Enum = 
        let PopoverState =
            Pattern.EnumStrings "PopoverState" ["auto"; "hint"; "manual"]

        let PopoverTargetAction =
            Pattern.EnumStrings "PopoverTargetAction" ["hide"; "show"; "toggle"]

        let ToggleState =
            Pattern.EnumStrings "ToggleState" ["open"; "closed"]

    let ToggleEventInit =
        Pattern.Config "ToggleEventInit" {
            Required = []
            Optional = [
                "newState", Enum.ToggleState.Type
                "oldState", Enum.ToggleState.Type
            ]
        }

    let ToggleEvent =
        Class "ToggleEvent"
        |=> Inherits T<Dom.Event>
        |+> Instance [
            "newState" =? Enum.ToggleState.Type
            "oldState" =? Enum.ToggleState.Type
        ]
        |+> Static [
            Constructor (T<string>?eventType * !?ToggleEventInit?init)
        ]     

    let ShowPopoverOptions =
        Pattern.Config "ShowPopoverOptions" {
            Required = []
            Optional = ["source", T<HTMLElement>]
        }

    let TogglePopoverOptions =
        Pattern.Config "TogglePopoverOptions" {
            Required = []
            Optional = [
                "force", T<bool>
                "source", T<HTMLElement>
            ]
        }

    let HTMLElement =
        Class "HTMLElement"
        |+> Instance [
            "popover" =@ Enum.PopoverState

            "showPopover" => !?ShowPopoverOptions ^-> T<unit>
            "hidePopover" => T<unit> ^-> T<unit>
            "togglePopover" => !?T<bool>?force * !?TogglePopoverOptions?source ^-> T<bool>

            // events
            "onbeforetoggle" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnBeforeToggle instead"
            "onbeforetoggle" =@ ToggleEvent ^-> T<unit>
            |> WithSourceName "OnBeforeToggle"

            "ontoggle" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnToggle instead"
            "ontoggle" =@ ToggleEvent ^-> T<unit>
            |> WithSourceName "OnToggle"
        ]        

    let HTMLButtonElement =
        Class "HTMLButtonElement"
        |+> Instance [
            "popoverTargetElement" =@ T<Dom.Element>
            "popoverTargetAction" =@ Enum.PopoverTargetAction
        ]        

    let HTMLInputElement =
        Class "HTMLInputElement"
        |+> Instance [
            "popoverTargetElement" =@ T<Dom.Element>
            "popoverTargetAction" =@ Enum.PopoverTargetAction
        ]
        
    let Assembly =
        Assembly [
            Namespace "WebSharper.Popover" [
                HTMLInputElement
                HTMLButtonElement
                HTMLElement
                TogglePopoverOptions
                ShowPopoverOptions
                ToggleEvent
                ToggleEventInit
                Enum.ToggleState
                Enum.PopoverTargetAction
                Enum.PopoverState
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
