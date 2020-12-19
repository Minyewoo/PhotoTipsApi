var images = $(".module img");
console.log(images);
var currentImg;
var prevImg;
var nextImg;

function setCurrent(id) {
  currentImg = $("#" + id);
  let prev = currentImg.prev();
  let next = currentImg.next();
  if (prev.length) {
    prevImg = prev;
  } else {
    prevImg = $("#" + images.get(-1).id);
  }
  if (next.length) {
    nextImg = next;
  } else {
    nextImg = $("#" + images.get(0).id);
  }
}

function delCurrent() {
  currentImg = null;
  prevImg = null;
  nextImg = null;
}

function openImg(clickedId) {
  setCurrent(clickedId);
  var source = currentImg.attr("src");
  $("#modalImg").attr("src", source);
  $(".modal").css("display", "block");
}

function closeImg() {
  delCurrent();
  $(".modal").css("display", "none");
}

function getPrevImg() {
  var prevSrc = prevImg.attr("src");
  $("#modalImg").attr("src", prevSrc);
  setCurrent(prevImg.attr("id"));
}

function getNextImg() {
  var nextSrc = nextImg.attr("src");
  $("#modalImg").attr("src", nextSrc);
  setCurrent(nextImg.attr("id"));
}

window.addEventListener(
  "keydown",
  function (event) {
    if (event.defaultPrevented) {
      return; // Do nothing if the event was already processed
    }
    switch (event.key) {
      case "Left": // IE/Edge specific value
      case "ArrowLeft":
        getPrevImg();
        break;
      case "Right": // IE/Edge specific value
      case "ArrowRight":
        getNextImg();
        break;
      case "Esc": // IE/Edge specific value
      case "Escape":
        closeImg();
        break;
      default:
        return; // Quit when this doesn't handle the key event.
    }
    // Cancel the default action to avoid it being handled twice
    event.preventDefault();
  },
  true
);

// var modal = document.getElementsByClassName("modal");
// document.body.addEventListener("click", (evt) => {
//   const modal = document.getElementById("modal");
//   window.onclick = function (event) {
//     if (event.target !== modal) {
//       modal.style.display = "none";
//     }
//   };
// });

// document.addEventListener("click", (evt) => {
//   let targetElement = evt.target; // clicked element
//   const modal = document.getElementById("modal");
//   do {
//     if (targetElement == modal) {
//       // This is a click inside. Do nothing, just return.
//       //   document.getElementById("flyout-debug").textContent = "Clicked inside!";
//       return;
//     }
//     // Go up the DOM
//     targetElement = targetElement.parentNode;
//   } while (targetElement);

//   // This is a click outside.
//   //   document.getElementById("flyout-debug").textContent = "Clicked outside!";
//   console.log("Clicked outside!");
//   modal.style.display = "none";
// });

// $(".gallery .row img").click(function () {
//   $(".modal").css("display", "block");
// });

// $(document).click(function (event) {
//   //if you click on anything except the modal itself or the "open modal" link, close the modal
//   if (!$(event.target).closest(".modal").length) {
//     $("body").find(".modal").css("display", "none");
//   }
// });
