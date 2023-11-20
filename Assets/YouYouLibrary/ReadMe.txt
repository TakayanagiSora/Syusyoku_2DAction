using YouYouLibrary.SortSearchSystem
要素のソートやサーチを行うためのものです。
自作するのが面倒な時に使用してください。（自作したほうが処理速度は速いです）

Sort

Sort.SortByArrangement_AscendingOrder_TypeStructOnly([] subjectArrangements)
引数に整数型、浮動小数点型などの値型配列を必要とします。
要素を昇順に並べ替える機能です。

Sort.SortByArrangement_AscendingOrder([] subjectArrangements)
引数に参照型配列も使用できます。
要素を昇順に並べ替える機能です。
通常はTypeStructOnlyを使用することを推奨します。



Sort.SortByList_AscendingOrder_TypeStructOnly(List subjectLists)
引数に整数型、浮動小数点型などの値型リストを必要とします。
要素を昇順に並べ替える機能です。

Sort.SortByList_AscendingOrder(List subjectLists)
引数に参照型リストも使用できます。
要素を昇順に並べ替える機能です。
通常はTypeStructOnlyを使用することを推奨します。



Sort.SortByArrangement_DescendingOrder_TypeStructOnly([] subjectArrangements)
引数に整数型、浮動小数点型などの値型配列を必要とします。
要素を降順に並べ替える機能です。

Sort.SortByArrangement_DescendingOrder([] subjectArrangements)
引数に参照型配列も使用できます。
要素を降順に並べ替える機能です。
通常はTypeStructOnlyを使用することを推奨します。



Sort.SortByList_DescendingOrder_TypeStructOnly(List subjectLists)
引数に整数型、浮動小数点型などの値型リストを必要とします。
要素を降順に並べ替える機能です。

Sort.SortByList_DescendingOrder(List subjectLists)
引数に参照型リストも使用できます。
要素を降順に並べ替える機能です。
通常はTypeStructOnlyを使用することを推奨します。

------------------------------------------------------------------------------------------------

Search

Search.SearchByArrangement_Matchs([] subjectArrangements, matchTarget)
matchTargetと一致しているすべての要素を取得できます。

Search.SearchByList_Matchs(List subjectLists, matchTarget)
matchTargetと一致しているすべての要素を取得できます。



Search.SearchByArrangement_MatchStruct_TypeDescendingSearch([] subjectArrangements, matchTarget)
引数に整数型、浮動小数点型などの値型配列を必要とします。
降順で探索します。最初にmatchTargetと一致したものを取得できます。

Search.SearchByArrangement_MatchClass_TypeDescendingSearch([] subjectArrangements, matchTarget)
引数に参照型配列も使用できます。
降順で探索します。最初にmatchTargetと一致したものを取得できます。



Search.SearchByList_MatchStruct_TypeDescendingSearch(List  subjectLists, matchTarget)
引数に整数型、浮動小数点型などの値型リストを必要とします。
降順で探索します。最初にmatchTargetと一致したものを取得できます。

Search.SearchByList_MatchClass_TypeDescendingSearch(List subjectLists, matchTarget)
引数に参照型リストも使用できます。
降順で探索します。最初にmatchTargetと一致したものを取得できます。



Search.SearchByArrangement_MatchStruct_TypeAscendingSearch([] subjectArrangements, matchTarget)
引数に整数型、浮動小数点型などの値型配列を必要とします。
昇順で探索します。最初にmatchTargetと一致したものを取得できます。

Search.SearchByArrangement_MatchClass_TypeAscendingSearch([] subjectArrangements, matchTarget)
引数に参照型配列も使用できます。
昇順で探索します。最初にmatchTargetと一致したものを取得できます。



Search.SearchByList_MatchStruct_TypeAscendingSearch(List  subjectLists, matchTarget)
引数に整数型、浮動小数点型などの値型リストを必要とします。
昇順で探索します。最初にmatchTargetと一致したものを取得できます。

Search.SearchByList_MatchClass_TypeAscendingSearch(List subjectLists, matchTarget)
引数に参照型リストも使用できます。
昇順で探索します。最初にmatchTargetと一致したものを取得できます。



Search.SearchByArrangement_MatchStruct_TypeDichotomySearch([] subjectArrangements, matchTarget)
引数に整数型、浮動小数点型などの値型配列を必要とします。
要素がソートされている状態である必要があります。
2分探索によるサーチをおこないます。

Search.SearchByList_MatchStruct_TypeDichotomySearch(List subjectLists, matchTarget)
引数に整数型、浮動小数点型などの値型リストを必要とします。
要素がソートされている状態である必要があります。
2分探索によるサーチをおこないます。

----------------------------------------------------------------------------------------


using YouYouLibrary.CipherSystem
文字列を暗号にするためのものです
ファイルにデータをセーブするときなどに使用できます。

Cipher

Cipher.Cipher_TypeA(string plaintext)
文字列を別な文字へ変換します。
復号方法はTypeAで暗号にした文字を再度代入すると戻ります。
string a = "あいうえお";
 a = Cipher.Cipher_TypeA(a); 暗号
 a = Cipher.Cipher_TypeA(a); 復号

----------------------------------------------------------------------------------------

using ListSystem
型に関係なくリストに追加できる


