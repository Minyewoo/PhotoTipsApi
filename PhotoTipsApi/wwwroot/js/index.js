function openFull(clickedId) {
  $("#" + clickedId)
    .closest(".module")
    .toggleClass("close");
}
