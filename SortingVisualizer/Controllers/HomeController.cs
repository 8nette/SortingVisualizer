using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SortingVisualizer.Models;

namespace SortingVisualizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static int[] array;
        private static List<SwapModel> swapList;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (array == null)
                resetArray();

            return View(array);
        }

        public IActionResult QuickSort()
        {
            if (array == null)
                resetArray();

            return View(array);
        }

        public IActionResult HeapSort()
        {
            if (array == null)
                resetArray();

            return View(array);
        }

        public IActionResult BubbleSort()
        {
            if (array == null)
                resetArray();

            return View(array);
        }

        public JsonResult GetJsonMergeSortSwapArray()
        {
            mergeSort();
            return Json(swapList);
        }

        public JsonResult GetJsonQuickSortSwapArray()
        {
            quickSort();
            return Json(swapList);
        }

        public JsonResult GetJsonHeapSortSwapArray()
        {
            heapSort();
            return Json(swapList);
        }

        public JsonResult GetJsonBubbleSortSwapArray()
        {
            bubbleSort();
            return Json(swapList);
        }

        public IActionResult ResetArray()
        {
            resetArray();
            return RedirectToAction("Index");
        }

        public IActionResult QuickSortResetArray()
        {
            resetArray();
            return RedirectToAction("QuickSort");
        }

        public IActionResult HeapSortResetArray()
        {
            resetArray();
            return RedirectToAction("HeapSort");
        }

        public IActionResult BubbleSortResetArray()
        {
            resetArray();
            return RedirectToAction("BubbleSort");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void resetArray()
        {
            int min = 5;
            int max = 100;

            array = new int[100];

            Random randNum = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = randNum.Next(min, max);
            }
        }

        private void mergeSort()
        {
            swapList = new List<SwapModel>();
            mSortArray(0, 99);
        }

        private void mSortArray(int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                mSortArray(left, middle);
                mSortArray(middle + 1, right);

                mMergeArrays(left, middle, right);
            }
        }

        private void mMergeArrays(int left, int middle, int right)
        {
            List<SwapModel> tempList = new List<SwapModel>();

            if (left == right)
            {
                return;
            }
            else if (right - left == 1)
            {
                if (array[left] <= array[right])
                {
                    return;
                }
                else
                {
                    int temp1;
                    int temp2;

                    temp1 = array[left];
                    temp2 = array[right];

                    array[left] = temp2;
                    array[right] = temp1;

                    tempList.Add(new SwapModel() { swap1 = left, swap2 = right });
                }
            }
            else
            {
                int i = left;
                int j = middle + 1;
                int idx = 0;
                int[] temp;
                List<(int arrayIdx, int value)> indexes = new List<(int arrayIdx, int value)>();
                temp = new int[right - left + 1];

                while (i <= middle && j <= right)
                {
                    if (array[i] <= array[j])
                    {
                        temp[idx] = array[i];
                        indexes.Add((arrayIdx: i, value: array[i]));
                        idx++;
                        i++;
                    }
                    else
                    {
                        temp[idx] = array[j];
                        indexes.Add((arrayIdx: j, value: array[j]));
                        idx++;
                        j++;
                    }
                }

                while (i <= middle)
                {
                    temp[idx] = array[i];
                    indexes.Add((arrayIdx: i, value: array[i]));
                    idx++;
                    i++;
                }

                while (j <= right)
                {
                    temp[idx] = array[j];
                    indexes.Add((arrayIdx: j, value: array[j]));
                    idx++;
                    j++;
                }

                idx = left;
                int tempIdx1;
                int tempIdx2;
                (int arrayIdx, int value) elementk;
                (int arrayIdx, int value) element2;

                int length = temp.Length;
                int count = indexes.Count;

                for (int k = 0; k < temp.Length; k++)
                {
                    array[idx] = temp[k];
                    elementk = indexes.ElementAt(k);
                    tempList.Add(new SwapModel() { swap1 = idx, swap2 = elementk.arrayIdx });

                    if (idx != elementk.arrayIdx)
                    {
                        element2 = indexes.Find(x => x.arrayIdx == idx);
                        int indexOfElement2 = indexes.IndexOf(element2);

                        tempIdx1 = elementk.arrayIdx;
                        tempIdx2 = element2.arrayIdx;

                        indexes[k] = (arrayIdx: tempIdx2, value: elementk.value);
                        indexes[indexOfElement2] = (arrayIdx: tempIdx1, value: element2.value);
                    }

                    idx++;
                }
            }

            swapList.AddRange(tempList);
        }

        private void quickSort()
        {
            int[] before = array;

            swapList = new List<SwapModel>();
            qSortArray(0, 99);

            int[] after = array;
        }

        private void qSortArray(int start, int end)
        {
            if (start < end)
            {
                int pivot_index = qPartition(start, end);

                if (pivot_index > 1)
                    qSortArray(start, pivot_index - 1);
                if (pivot_index + 1 < end)
                    qSortArray(pivot_index + 1, end);
            }
        }

        private int qPartition(int start, int end)
        {
            List<SwapModel> tempList = new List<SwapModel>();
            int pivot = array[end];

            int i = start - 1;
            for (int j = start; j <= end - 1; j++)
            {
                if (array[j] <= pivot)
                {
                    i++;
                    tempList.Add(qSwap(i, j));
                }
            }
            tempList.Add(qSwap(i + 1, end));
            swapList.AddRange(tempList);
            return i + 1;
        }

        private SwapModel qSwap(int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;

            return new SwapModel() { swap1 = i, swap2 = j };
        }

        private void heapSort()
        {
            swapList = new List<SwapModel>();

            for (int i = 100 / 2 - 1; i >= 0; i--)
            {
                heapify(100, i);
            }

            for (int i = 100 - 1; i >= 0; i--)
            {
                int temp = array[0];
                array[0] = array[i];
                array[i] = temp;

                swapList.Add(new SwapModel() { swap1 = 0, swap2 = i });

                heapify(i, 0);
            }
        }

        private void heapify(int size, int index)
        {
            int largestIndex = index;
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;

            if (leftChild < size && array[leftChild] > array[largestIndex])
                largestIndex = leftChild;

            if (rightChild < size && array[rightChild] > array[largestIndex])
                largestIndex = rightChild;

            if (largestIndex != index)
            {
                int temp = array[index];
                array[index] = array[largestIndex];
                array[largestIndex] = temp;

                swapList.Add(new SwapModel() { swap1 = index, swap2 = largestIndex });

                heapify(size, largestIndex);
            }
        }

        private void bubbleSort()
        {
            int[] before = array;

            swapList = new List<SwapModel>();

            for (int i = 0; i < array.Count(); i++)
            {
                for (int j = 0; j < array.Count() - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;

                        swapList.Add(new SwapModel() { swap1 = j, swap2 = j + 1 });
                    }
                }
            }

            int[] after = array;
        }
    }
}
