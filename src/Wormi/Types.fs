namespace Wormi

open System

[<Struct>]
type PostMetadata = {
  id: string
  post_id: string
  tags: string list
  slug: string
}

type Post = {
  id: string
  title: string
  content: string
  metadata: PostMetadata voption
  updated_at: DateTime voption
  created_at: DateTime
}
