# Ochi-ss

📝 Web見積システム（ASP.NET WebForms）

ASP.NET WebForms（C#）を使用した、顧客向けのWeb見積システムです。
取引先がオンラインで見積を作成・確認できる機能を提供します。

📌 主な機能
🏢 顧客管理：取引先の登録・編集・削除
📄 見積作成：商品を選択して見積を生成
📊 見積履歴：過去の見積を一覧表示
📧 メール送信：見積書をPDFでダウンロード・メール送信
🔒 認証機能：ユーザーログイン（ASP.NET Membership / Identity）
📂 管理画面：管理者向けの設定ページ

🚀 環境構築

1️⃣ 必要環境
.NET Framework ：4.7.2 以上
IIS / IIS Express
SQL Server（Express / Azure SQL など）

2️⃣ インストール
sh
git clone https://github.com/your-username/quotation-system.git
cd quotation-system

3️⃣ データベースのセットアップ
App_Data フォルダに quotation_db.mdf を配置
Web.config の接続文字列を設定
xml

<connectionStrings>
    <add name="QuotationDB" 
         connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\quotation_db.mdf;Integrated Security=True"
         providerName="System.Data.SqlClient" />
</connectionStrings>
データベースをマイグレーション（または手動作成）

4️⃣ 実行方法
Visual Studio で QuotationSystem.sln を開
Ctrl + F5 で実行
ブラウザで http://localhost:port/ を開く

📂 ディレクトリ構成
bash
コピーする
編集する
/QuotationSystem
│-- /App_Code         # ビジネスロジック（BLL / DAL）
│-- /App_Data         # データベースファイル
│-- /Content          # CSS, JavaScript, 画像ファイル
│-- /Pages            # Webフォーム（.aspx, .aspx.cs）
│-- /MasterPages      # マスターページ
│-- /bin              # DLLファイル
│-- Global.asax       # アプリケーションイベント管理
│-- Web.config        # 設定ファイル

💡 使い
🔹 見積の作成手順
ログイン する（ユーザー登録済みの場合）
「新しい見積を作成」ページに移動
商品を選択し、数量を入力
見積を保存して、PDFをダウンロードまたはメール送信

🔹 管理者機能
ユーザー管理
取引先管理
商品データの登録・更新

🔧 開発メモ
ASP.NET WebForms を使用（GridView で見積一覧、Repeater で商品リスト）
Entity Framework を利用した DB 操作
AJAX（UpdatePanel）を使用して部分更新
iTextSharp を用いたPDF出力

🤝 コントリビューション
このリポジトリをフォーク
feature-branch を作成 (git checkout -b feature-branch)
修正をコミット (git commit -m "Add new feature")
プルリクエストを作成

📄 ライセンス
このプロジェクトは MIT License のもとで公開されています。
