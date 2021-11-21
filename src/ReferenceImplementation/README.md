## Java implementation

Author: Josh Bloch

A stable, adaptive, iterative mergesort that requires far fewer than
n lg(n) comparisons when running on partially sorted arrays, while
offering performance comparable to a traditional mergesort when run
on random arrays.  Like all proper merge-sorts, this sort is stable and
runs O(n log n) time (worst case).  In the worst case, this sort requires
temporary storage space for n/2 object references; in the best case,
it requires only a small constant amount of space.

This implementation was adapted from Tim Peters's list sort for
Python, which is described in detail here:
http://svn.python.org/projects/python/trunk/Objects/listsort.txt

Tim's C code may be found here:
http://svn.python.org/projects/python/trunk/Objects/listobject.c

The underlying techniques are described in this paper (and may have
even earlier origins):

"Optimistic Sorting and Information Theoretic Complexity"
Peter McIlroy
SODA (Fourth Annual ACM-SIAM Symposium on Discrete Algorithms),
pp 467-474, Austin, Texas, 25-27 January 1993.

While the API to this class consists solely of static methods, it is
(privately) instantiable; a TimSort instance holds the state of an ongoing
sort, assuming the input array is large enough to warrant the full-blown
TimSort. Small arrays are sorted in place, using a binary insertion sort.

## C# port

Author: Milosz Krajewski

This implementation was adapted from Josh Bloch array sort for Java which can be found here:
http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/main/java/util/TimSort.java?revision=1.5&view=markup

All modifications are licensed using Apache License (same as original Java implementation)
