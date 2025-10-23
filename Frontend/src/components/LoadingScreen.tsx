import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

export default function LoadingScreen() {
  return (
    <div className="min-h-screen grid place-items-center bg-background">
      <Card className="w-full max-w-sm">
        <CardHeader>
          <CardTitle>Yükleniyor</CardTitle>
        </CardHeader>
        <CardContent className="py-6 text-center text-muted-foreground">
          <div className="mx-auto mb-3 h-8 w-8 rounded-full border-2 border-muted-foreground/40 border-t-foreground animate-spin" />
          Oturum doğrulanıyor...
        </CardContent>
      </Card>
    </div>
  )
}