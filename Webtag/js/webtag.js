
$(function ()
{
    $("header .fa-bars").click(function ()
    {
        $("#mobile-nav").slideToggle();
    });
});

var Dashboard = function (saveLinkUrl, saveFolderUrl, delLinkUrl, delFolderUrl, sortUrl)
{
    var reordering = false;
    var orderingInited = false;

    function closeInputs()
    {
        if ($("#link-inputs").is(":visible"))
        {
            $("#link-inputs input[type='text']").removeClass("input-validation-error");
            $("#link-inputs").slideUp();
            $("#link-url-validation").hide();
        }

        if ($("#folder-inputs").is(":visible"))
        {
            $("#folder-inputs input[type='text']").removeClass("input-validation-error");
            $("#folder-inputs").slideUp();
        }
    }

    function saveLink()
    {
        var valid = true;

        if ($.trim($("#link-url").val().substring(0, 4)) != "http")
        {
            $("#link-url").val("http://" + $("#link-url").val());
        }

        if ($.trim($("#link-title").val()) == "")
        {
            valid = false;
            $("#link-title").addClass("input-validation-error");
        }
        if ($.trim($("#link-url").val()) == "")
        {
            valid = false;
            $("#link-url").addClass("input-validation-error");
        }
        else if (!isUrlValid($("#link-url").val()))
        {
            valid = false;
            $("#link-url").addClass("input-validation-error");
            $("#link-url-validation").show();
        }

        if(valid)
        {
            $.post(saveLinkUrl, { title: $("#link-title").val(), href: $("#link-url").val(), id: $("#link-id").val() }, function (data)
            {
                updateLinkList(data);
                closeInputs();
            });
        }
    }

    function saveFolder()
    {
        var valid = true;

        if ($.trim($("#folder-name").val()) == "")
        {
            valid = false;
            $("#folder-name").addClass("input-validation-error");
        }

        if (valid)
        {
            $.post(saveFolderUrl, { name: $("#folder-name").val(), id: $("#folder-id").val() }, function (data)
            {
                updateLinkList(data);
                closeInputs();
            });
        }
    }

    function updateLinks()
    {
        $(".link, .folder").each(function ()
        {
            var node = $(this);

            if (node.hasClass("link"))
            {
                // update each link with favicons
                var link = $(this).find("a");
                var faviconURL = link.attr('href').replace(/^(http:\/\/[^\/]+).*$/, '$1') + '/favicon.ico';

                $('<img src="' + faviconURL + '" />').load(function ()
                {
                    link.find("i.fa").replaceWith($(this));
                });
            }
            else if (node.hasClass("folder"))
            {
                node.find(">a").click(function (e)
                {
                    e.preventDefault();
                    node.find(".links").slideToggle();
                });
            }

            node.next(".link-buttons").find(".edit-button").click(function ()
            {
                var btn = $(this);
                if(btn.data("folder") == true)
                {
                    showFolderForm(btn.data("id"), btn.data("name"));
                }
                else
                {
                    showLinkForm(btn.data("id"), btn.data("name"), btn.data("url"));
                }
            });

            node.next(".link-buttons").find(".delete-button").hover(
                function ()
                {
                    node.addClass("strike-through");
                },
                function ()
                {
                    node.removeClass("strike-through");
                }
            ).click(function ()
            {
                stopOrdering();
                var btn = $(this);
                var isFolder = (btn.data("folder") == true);
                if (confirm("Delete this " + (isFolder ? "folder" : "link") + "?"))
                {
                    var delUrl = isFolder ? delFolderUrl : delLinkUrl;
                    $.post(delUrl, { id: btn.data("id") }, function (data)
                    {
                        updateLinkList(data);
                    });
                }
            });
        });
    }

    function updateLinkList(linkHtml)
    {
        $("#link-list").html(linkHtml);
        orderingInited = false;
        updateLinks();
    }

    function startOrdering()
    {
        reordering = true;
        closeInputs();
        $(".reorder-links").html("<span>Stop ordering</span>");
        $(".order-handle").show();
        if (orderingInited)
        {
            $(".links").sortable("enable");
        }
        else
        {
            orderingInited = true;

            $(".links").sortable({
                items: ">li",
                connectWith: ".links",
                stop: function (event, ui)
                {
                    var parentId = 0;
                    var sortable = ui.item.parent();
                    var sortableLi = sortable.parents("li");
                    if (sortableLi.length > 0)
                    {
                        parentId = sortableLi.prop("id").substring(5);
                    }
                    var childIds = sortable.sortable("serialize", { key: "link" });
                    $.post(sortUrl, { parentId: parentId, childIds: childIds });
                }
            });
        }
        $(".links li .folder .links").show();
    }

    function stopOrdering()
    {
        if (reordering)
        {
            reordering = false;
            $(".reorder-links").html("<span>Reorder</span>");
            $(".order-handle").hide();
            $(".links").sortable("disable");
            $(".links li .folder .links").hide();
        }
    }

    function showFolderForm(folderId, folderName)
    {
        stopOrdering();
        if ($("#link-inputs").is(":visible"))
        {
            $("#link-inputs").slideUp();
        }
        if (typeof (folderId) == "undefined")
        {
            $("#folder-id").val("");
            $("#folder-name").val("");
        }
        else
        {
            $("#folder-id").val(folderId);
            $("#folder-name").val(folderName);
        }
        $("#folder-inputs").slideDown();
        $("#folder-name").focus();
    }

    function showLinkForm(linkId, linkTitle, linkUrl)
    {
        stopOrdering();
        if ($("#folder-inputs").is(":visible"))
        {
            $("#folder-inputs").slideUp();
        }
        if (typeof (linkId) == "undefined")
        {
            $("#link-id").val("");
            $("#link-title").val("");
            $("#link-url").val("");
        }
        else
        {
            $("#link-id").val(linkId);
            $("#link-title").val(linkTitle);
            $("#link-url").val(linkUrl);
        }
        $("#link-inputs").slideDown();
        $("#link-title").focus();
    }

    $(".add-link").click(function()
    {
        showLinkForm();
    });

    $(".add-folder").click(function ()
    {
        showFolderForm();       
    });

    $("#link-inputs input[type='text']").keyup(function (e)
    {
        $(this).removeClass("input-validation-error");
        $("#link-url-validation").hide();

        if (e.keyCode == 27)
        {
            closeInputs();
        }
        else if (e.keyCode == 13)
        {
            saveLink();
        }
    });

    $("#folder-inputs input[type='text']").keyup(function (e)
    {
        $(this).removeClass("input-validation-error");

        if (e.keyCode == 27)
        {
            closeInputs();
        }
        else if (e.keyCode == 13)
        {
            saveFolder();
        }
    });
    
    $("#cancel-link-button, #cancel-folder-button").click(function ()
    {
        closeInputs();
    });

    $("#save-link-button").click(function ()
    {
        saveLink();
    });

    $("#save-folder-button").click(function ()
    {
        saveFolder();
    });

    $(".reorder-links").click(function ()
    {
        if (reordering)
        {
            stopOrdering();
        }
        else
        {
            startOrdering();
        }
    });

    updateLinks();
}

function isUrlValid(url)
{
    return /^(https?|s?ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url);
}
